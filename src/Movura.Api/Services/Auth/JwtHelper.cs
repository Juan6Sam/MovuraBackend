using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Movura.Api.Constants;
using Movura.Api.Data.Entities;

namespace Movura.Api.Services.Auth;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        var tokenValidityInMinutes = _configuration.GetValue<int>("JWT:TokenValidityInMinutes");
        var effectiveRole = user.Role?.Nombre ?? UserRoles.Cliente;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, effectiveRole)
        };

        // Agregar permisos basados en el rol
        switch (effectiveRole)
        {
            case UserRoles.Admin:
                claims.AddRange(new[]
                {
                    new Claim(Permissions.ManageUsers, "true"),
                    new Claim(Permissions.ManageParkings, "true"),
                    new Claim(Permissions.ManageComercios, "true"),
                    new Claim(Permissions.ViewReports, "true"),
                    new Claim(Permissions.ManualPayment, "true")
                });
                break;
            case UserRoles.Comercio:
                claims.AddRange(new[]
                {
                    new Claim(Permissions.ManageComercios, "true"),
                    new Claim(Permissions.ViewReports, "true")
                });
                break;
            case UserRoles.Cliente:
                claims.Add(new Claim(Permissions.ViewReports, "true"));
                break;
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret not configured")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public DateTime GetRefreshTokenExpiryTime()
    {
        var refreshTokenValidityInDays = _configuration.GetValue<int>("JWT:RefreshTokenValidityInDays");
        return DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
    }
}