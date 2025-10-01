using System.Threading.Tasks;
using Movura.Api.Services;
using Movura.Api.Models.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Movura.Api.Data;

namespace Movura.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly MovuraDbContext _db;
        private readonly IConfiguration _cfg;
        public AuthService(MovuraDbContext db, IConfiguration cfg) { _db = db; _cfg = cfg; }

        public async Task<(bool Success, LoginResponse Data)> LoginAsync(string email, string password, string ip)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return (false, null);

            var valid = VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            if (!valid) return (false, null);

            var token = JwtHelper.GenerateToken(user, _cfg);
            var resp = new LoginResponse { Token = token, User = new LoginResponseUser { Username = user.Username, Email = user.Email, FirstLogin = user.FirstLogin } };
            return (true, resp);
        }

        public async Task<bool> ForgotPasswordAsync(string email, string ip)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            var token = Guid.NewGuid().ToString("N");
            if (user != null)
            {
                _db.PasswordResetTokens.Add(new Data.Entities.PasswordResetToken
                {
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                });
                await _db.SaveChangesAsync();
                Serilog.Log.Information("Password reset token {Token} for user {Email}", token, email);
            }
            return true;
        }

        public async Task<bool> ChangeFirstPasswordAsync(string username, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !user.FirstLogin) return false;
            var (hash, salt) = HashPassword(newPassword);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.FirstLogin = false;
            await _db.SaveChangesAsync();
            return true;
        }

        private (string hash, string salt) HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            using var derive = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            var hashBytes = derive.GetBytes(32);
            var hash = Convert.ToBase64String(hashBytes);
            return (hash, salt);
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                var saltBytes = Convert.FromBase64String(storedSalt);
                using var derive = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
                var hashBytes = derive.GetBytes(32);
                var hash = Convert.ToBase64String(hashBytes);
                return hash == storedHash;
            }
            catch { return false; }
        }
    }
}
