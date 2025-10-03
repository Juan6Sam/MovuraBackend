using FluentValidation;
using Movura.Api.Models.Dto;
using System.Linq;

namespace Movura.Api.Validators;

public class ComercioValidator : AbstractValidator<ComercioDto>
{
    public ComercioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo es requerido")
            .Must(tipo => tipo == "monto" || tipo == "tiempo")
            .WithMessage("El tipo debe ser 'monto' o 'tiempo'");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("El valor debe ser mayor que 0");
    }
}

public class BulkUpdateComerciosValidator : AbstractValidator<List<ComercioDto>>
{
    public BulkUpdateComerciosValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("La lista de comercios no puede estar vacía")
            .Must(comercios => comercios.Count <= 100)
            .WithMessage("No se pueden actualizar más de 100 comercios a la vez");

        RuleForEach(x => x).SetValidator(new ComercioValidator());
    }
}
