using System.Net;

namespace Domain.Exceptions;

public sealed class ValidacaoException : Exception
{
    public ValidacaoException(IReadOnlyList<string> notificacoes, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base("Ocorreram erros de validação.")
    {
        Notificacoes = notificacoes;
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public IReadOnlyList<string> Notificacoes { get; }
}