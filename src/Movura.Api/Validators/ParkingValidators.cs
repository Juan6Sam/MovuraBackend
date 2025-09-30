using FluentValidation;
using Movura.Api.Models.Dto;

namespace Movura.Api.Validators;

public class ParkingConfigDtoValidator : AbstractValidator<ParkingConfigDto>
{
    public ParkingConfigDtoValidator()
    {
        RuleFor(x => x.TarifaBase)
            .GreaterThanOrEqualTo(0).WithMessage("La tarifa base no puede ser negativa");

        RuleFor(x => x.CostoHora)
            .GreaterThanOrEqualTo(0).WithMessage("El costo por hora no puede ser negativo");

        RuleFor(x => x.FraccionMin)
            .GreaterThan(0).WithMessage("La fracción mínima debe ser mayor a 0 minutos");

        RuleFor(x => x.CostoFraccion)
            .GreaterThanOrEqualTo(0).WithMessage("El costo por fracción no puede ser negativo");

        RuleFor(x => x.GraciaMin)
            .GreaterThanOrEqualTo(0).WithMessage("El tiempo de gracia no puede ser negativo");

        RuleFor(x => x.HoraCorte)
            .Matches(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("El formato de hora de corte debe ser HH:mm");
    }
}