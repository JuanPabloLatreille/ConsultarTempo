namespace Domain.Interfaces.Services;

public interface ICriptografarSenhaService
{
    string Criptografar(string senha);

    bool Verificar(string senha, string senhaHash);
}