using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movura.Api.Constants;
using Movura.Api.Data.Context;
using Movura.Api.Data.Entities;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly MovuraDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        MovuraDbContext context,
        JwtHelper jwtHelper,
        IMapper mapper,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request)
    {
        // 1) Obtener salt activo del usuario
        // Validar con EF usando Contrasenas (salt + SHA-512)
        var userWithSecret = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Email == request.Email)
            .Select(u => new
            {
                User = u,
                Secret = _context.Contrasenas
                    .Where(c => c.UserId == u.Id && c.Activa)
                    .OrderByDescending(c => c.FechaCreacion)
                    .Select(c => new { c.Salt, c.Hash })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (userWithSecret?.Secret == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var computedHash = HashPassword(request.Password + userWithSecret.Secret.Salt);
        // Logs de depuración removidos
        if (!string.Equals(computedHash, userWithSecret.Secret.Hash, StringComparison.OrdinalIgnoreCase))
        {
            // Registrar intento fallido si corresponde
            var failedUser = userWithSecret.User;
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarIntentoAcceso @IdUsuario, @IpAddress, @Exitoso, @Descripcion",
                new SqlParameter("@IdUsuario", failedUser.Id),
                new SqlParameter("@IpAddress", string.Empty),
                new SqlParameter("@Exitoso", false),
                new SqlParameter("@Descripcion", "Credenciales inválidas"));
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var user = userWithSecret.User;

        if (user == null)
        {
            // Registrar el intento fallido
            var failedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (failedUser != null)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_RegistrarIntentoAcceso @IdUsuario, @IpAddress, @Exitoso, @Descripcion",
                    new SqlParameter("@IdUsuario", failedUser.Id),
                    new SqlParameter("@IpAddress", ""), // TODO: Get client IP from context
                    new SqlParameter("@Exitoso", false),
                    new SqlParameter("@Descripcion", "Credenciales inválidas"));
            }
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Registrar el intento exitoso
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_RegistrarIntentoAcceso @IdUsuario, @IpAddress, @Exitoso, @Descripcion",
            new SqlParameter("@IdUsuario", user.Id),
            new SqlParameter("@IpAddress", ""), // TODO: Get client IP from context
            new SqlParameter("@Exitoso", true),
            new SqlParameter("@Descripcion", "Login exitoso"));

        // Generar token JWT basado en entidad User
        var token = _jwtHelper.GenerateJwtToken(user);
        var userDto = _mapper.Map<UserDto>(user);

        return new AuthLoginResponse
        {
            Token = token,
            User = userDto,
            RefreshTokenCookie = null
        };
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            _logger.LogWarning("Password reset requested for non-existent user: {Email}", request.Email);
            return false;
        }

        var tempPassword = GenerateTemporaryPassword();

        var salt = Guid.NewGuid().ToString();
        var hash = HashPassword(tempPassword + salt);

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_ActualizarContrasena @IdUsuario, @NuevoHash, @NuevoSalt",
            new SqlParameter("@IdUsuario", user.Id),
            new SqlParameter("@NuevoHash", hash),
            new SqlParameter("@NuevoSalt", salt));

        var emailBody = $@"
            <h2>Password Reset</h2>
            <p>Your temporary password is: <strong>{tempPassword}</strong></p>
            <p>Please change this password when you next log in.</p>
            <p>If you did not request this password reset, please contact support immediately.</p>";

        await _emailService.SendEmailAsync(user.Email, "Password Reset", emailBody);
        
        return true;
    }

    public async Task<bool> ChangeFirstPasswordAsync(ChangeFirstPasswordRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Username && u.FirstLogin);

        if (user == null)
        {
            throw new InvalidOperationException("User not found or not eligible for first password change");
        }

        var salt2 = Guid.NewGuid().ToString();
        var hash2 = HashPassword(request.NewPassword + salt2);

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_ActualizarContrasena @IdUsuario, @NuevoHash, @NuevoSalt",
            new SqlParameter("@IdUsuario", user.Id),
            new SqlParameter("@NuevoHash", hash2),
            new SqlParameter("@NuevoSalt", salt2));
        return true;
    }

    private static string HashPassword(string password)
    {
        using var sha512 = SHA512.Create();
        // Importante: usar UTF-16 LE (Encoding.Unicode) para alinearnos con HASHBYTES(NVARCHAR)
        var hashedBytes = sha512.ComputeHash(Encoding.Unicode.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    private static string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}