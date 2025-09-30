using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);

            if (response.RefreshTokenCookie != null)
            {
                Response.Cookies.Append("RefreshToken", response.RefreshTokenCookie, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Login fallido para {Email}", request.Email);
            return Unauthorized(new ErrorResponse { Message = "Credenciales inválidas" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login para {Email}", request.Email);
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }

    [HttpPost("forgot")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var result = await _authService.ForgotPasswordAsync(request);
            return Ok(new { success = result, message = "Si el email existe, recibirás instrucciones para restablecer tu contraseña" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en recuperación de contraseña para {Email}", request.Email);
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }

    [HttpPost("change-first-password")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeFirstPassword([FromBody] ChangeFirstPasswordRequest request)
    {
        try
        {
            var result = await _authService.ChangeFirstPasswordAsync(request);
            return Ok(new { success = result, message = "Contraseña actualizada correctamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cambio de contraseña fallido para {Username}", request.Username);
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en cambio de contraseña para {Username}", request.Username);
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }
}