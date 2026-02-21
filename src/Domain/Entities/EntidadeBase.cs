namespace Domain.Entities;

public abstract class EntidadeBase
{
    internal EntidadeBase()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        Deletado = false;
    }

    public Guid Id { get; protected set; }

    public DateTime CriadoEm { get; protected set; }

    public DateTime? AtualizadoEm { get; protected set; }

    public bool Deletado { get; protected set; }

    public void MarcarComoDeletado()
    {
        Deletado = true;
        AtualizadoEm = DateTime.UtcNow;
    }
}