using FluentValidation;

namespace Application.Cidades.Commands.CadastrarCidade;

public class CadastrarCidadeCommandValidator : AbstractValidator<CadastrarCidadeCommand>
{
    public CadastrarCidadeCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(c => c.Uf)
            .NotEmpty().WithMessage("UF é obrigatória.")
            .Length(2).WithMessage("UF deve ter exatamente 2 caracteres.");

        RuleFor(c => c.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude deve estar entre -90 e 90.");

        RuleFor(c => c.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude deve estar entre -180 e 180.");
    }
}