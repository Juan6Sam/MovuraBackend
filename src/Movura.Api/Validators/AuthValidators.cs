using FluentValidation;
using Movura.Api.Models.Dto;

namespace Movura.Api.Validators;

public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
{
    public AuthLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
    }
}

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido");
    }
}

public class ChangeFirstPasswordRequestValidator : AbstractValidator<ChangeFirstPasswordRequest>
{
    public ChangeFirstPasswordRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La nueva contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula")
            .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial");
    }
}