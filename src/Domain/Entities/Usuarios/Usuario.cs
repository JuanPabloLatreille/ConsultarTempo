namespace Domain.Entities.Usuarios;

public sealed class Usuario : EntidadeBase
{
    private Usuario()
    {
    }

    private Usuario(string nome, string senha)
    {
        Nome = nome;
        Senha = senha;
    }

    public string Nome { get; private set; }

    public string Senha { get; private set; }

    public static Usuario Criar(string nome, string senha)
    {
        return new Usuario(nome, senha);
    }
}