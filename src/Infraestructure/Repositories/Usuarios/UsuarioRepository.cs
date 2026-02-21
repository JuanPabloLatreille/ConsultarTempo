using Domain.Entities.Usuarios;
using Domain.Interfaces.Usuarios;
using Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationContext _context;

    public UsuarioRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Usuario>()
            .FirstOrDefaultAsync(u => u.Nome == nome, cancellationToken);
    }

    public async Task<List<Usuario>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Usuario>()
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await _context.Set<Usuario>().AddAsync(usuario, cancellationToken);
    }
}