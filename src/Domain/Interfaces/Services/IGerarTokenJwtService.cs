namespace Domain.Interfaces.Services;

public interface IGerarTokenJwtService
{
    string Gerar(Guid usuarioId, string nome);
}