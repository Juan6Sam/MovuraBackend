namespace Movura.Api.Models.Dto;

public class AuthLoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AuthLoginResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
    public string? RefreshTokenCookie { get; set; }
}

public class UserDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool FirstLogin { get; set; }
}

public class ForgotPasswordRequest
{
    public string Email { get; set; } = null!;
}

public class ChangeFirstPasswordRequest
{
    public string Username { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

public class ErrorResponse
{
    public string Message { get; set; } = null!;
}