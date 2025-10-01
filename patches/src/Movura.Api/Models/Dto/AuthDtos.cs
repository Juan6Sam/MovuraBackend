namespace Movura.Api.Models.Dto
{
    public class LoginRequest { public string Email { get; set; } public string Password { get; set; } }
    public class ForgotRequest { public string Email { get; set; } }
    public class ChangeFirstPasswordRequest { public string Username { get; set; } public string NewPassword { get; set; } }

    public class LoginResponseUser { public string Username { get; set; } public string Email { get; set; } public bool FirstLogin { get; set; } }
    public class LoginResponse { public string Token { get; set; } public LoginResponseUser User { get; set; } }
}
