using Domain.Entities.Usuarios;

namespace Domain.Interfaces.Usuarios;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorNomeAsync(string nome, CancellationToken cancellationToken = default);
    
    Task<List<Usuario>> ObterTodosAsync(CancellationToken cancellationToken = default);
    
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);
}