using Domain.Entities.Cidades;

namespace Domain.Interfaces.Cidades;

public interface ICidadeRepository
{
    Task<Cidade?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Cidade?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);

    Task<Cidade?> ObterPorCoordenadasAsync(double latitude, double longitude, CancellationToken cancellationToken = default);

    Task<List<Cidade>> ObterTodosAsync(CancellationToken cancellationToken = default);

    Task AdicionarAsync(Cidade cidade, CancellationToken cancellationToken = default);
}