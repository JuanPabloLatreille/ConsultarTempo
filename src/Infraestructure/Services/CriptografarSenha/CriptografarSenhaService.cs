using Domain.Interfaces.Services;

namespace Infraestructure.Services.CriptografarSenha;

public class CriptografarSenhaService : ICriptografarSenhaService
{
    public string Criptografar(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool Verificar(string senha, string senhaHash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
    }
}