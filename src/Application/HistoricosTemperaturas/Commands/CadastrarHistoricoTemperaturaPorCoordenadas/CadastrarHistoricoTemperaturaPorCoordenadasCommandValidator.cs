using FluentValidation;

namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperaturaPorCoordenadas;

public class
    CadastrarHistoricoTemperaturaPorCoordenadasCommandValidator : AbstractValidator<
    CadastrarHistoricoTemperaturaPorCoordenadasCommand>
{
    public CadastrarHistoricoTemperaturaPorCoordenadasCommandValidator()
    {
        RuleFor(c => c.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude deve estar entre -90 e 90.");

        RuleFor(c => c.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude deve estar entre -180 e 180.");
    }
}