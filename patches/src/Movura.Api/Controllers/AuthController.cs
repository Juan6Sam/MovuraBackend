using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Movura.Api.Services;
using Movura.Api.Models.Dto;
using System.Threading.Tasks;

namespace Movura.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _auth.LoginAsync(req.Email, req.Password, HttpContext.Connection.RemoteIpAddress?.ToString());
            if (!result.Success) return Unauthorized(new { success = false, error = new { code = "AUTH_INVALID", message = "Credenciales inválidas" } });
            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotRequest req)
        {
            var res = await _auth.ForgotPasswordAsync(req.Email, HttpContext.Connection.RemoteIpAddress?.ToString());
            return Ok(new { success = true, data = new { success = res } });
        }

        [HttpPost("change-first-password")]
        [Authorize]
        public async Task<IActionResult> ChangeFirstPassword([FromBody] ChangeFirstPasswordRequest req)
        {
            var username = User.Identity?.Name ?? req.Username;
            var ok = await _auth.ChangeFirstPasswordAsync(username, req.NewPassword);
            if (!ok) return BadRequest(new { success = false, error = new { code = "CHANGE_FAIL", message = "Cambio no permitido o inválido" } });
            return Ok(new { success = true });
        }
    }
}
