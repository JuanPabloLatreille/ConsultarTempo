using Domain.Entities.HistoricosTemperaturas;
using Domain.Interfaces.HistoricosTemperaturas;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories.HistoricosTemperaturas;

public class HistoricoTemperaturaRepository : IHistoricoTemperaturaRepository
{
    private readonly ApplicationContext _context;

    public HistoricoTemperaturaRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<HistoricoTemperatura?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<HistoricoTemperatura>()
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public async Task<List<HistoricoTemperatura>> ObterPorCidadeIdAsync(Guid cidadeId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<HistoricoTemperatura>()
            .Include(x => x.Cidade)
            .Where(h => h.CidadeId == cidadeId)
            .OrderByDescending(h => h.DataRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<HistoricoTemperatura>> ObterPorCidadeIdUltimos30DiasAsync(Guid cidadeId,
        CancellationToken cancellationToken = default)
    {
        var dataLimite = DateTime.UtcNow.AddDays(-30);

        return await _context.Set<HistoricoTemperatura>()
            .Include(x => x.Cidade)
            .Where(h => h.CidadeId == cidadeId && h.DataRegistro >= dataLimite)
            .OrderByDescending(h => h.DataRegistro)
            .ToListAsync(cancellationToken);
    }

    public Task<List<HistoricoTemperatura>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return _context.Set<HistoricoTemperatura>()
            .Include(x => x.Cidade)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(HistoricoTemperatura historico, CancellationToken cancellationToken = default)
    {
        await _context.Set<HistoricoTemperatura>().AddAsync(historico, cancellationToken);
    }

    public void Remover(HistoricoTemperatura historico)
    {
        historico.MarcarComoDeletado();
    }
}