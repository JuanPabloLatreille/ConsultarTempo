using Domain.Entities.Cidades;
using Domain.Interfaces.Cidades;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories.Cidades;

public class CidadeRepository : ICidadeRepository
{
    private readonly ApplicationContext _context;

    public CidadeRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Cidade?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cidade>()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cidade?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cidade>()
            .FirstOrDefaultAsync(c => c.Nome == nome, cancellationToken);
    }

    public async Task<Cidade?> ObterPorCoordenadasAsync(double latitude, double longitude,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cidade>()
            .FirstOrDefaultAsync(c => c.Latitude == latitude && c.Longitude == longitude, cancellationToken);
    }

    public async Task<List<Cidade>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cidade>()
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Cidade cidade, CancellationToken cancellationToken = default)
    {
        await _context.Set<Cidade>().AddAsync(cidade, cancellationToken);
    }
}