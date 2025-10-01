using System.Threading.Tasks;
using Movura.Api.Models.Dto;

namespace Movura.Api.Services
{
    public interface IAuthService
    {
        Task<(bool Success, LoginResponse Data)> LoginAsync(string email, string password, string ip);
        Task<bool> ForgotPasswordAsync(string email, string ip);
        Task<bool> ChangeFirstPasswordAsync(string username, string newPassword);
    }
}
