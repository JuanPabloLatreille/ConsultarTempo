using FluentValidation;

namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperatura;

public class CadastrarHistoricoTemperaturaCommandValidator : AbstractValidator<CadastrarHistoricoTemperaturaCommand>
{
    public CadastrarHistoricoTemperaturaCommandValidator()
    {
        RuleFor(x => x.NomeCidade)
            .NotEmpty().WithMessage("O nome da cidade é obrigatório.")
            .MaximumLength(200).WithMessage("O nome da cidade deve conter no máximo 200 caracteres.");
    }
}