using System.Net;
using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Interfaces.Usuarios;
using MediatR;

namespace Application.Autenticacao.RealizarAutenticacao;

public sealed record RealizarAutenticacaoCommand(string Nome, string Senha) : IRequest<string>;

public sealed class RealizarAutenticacaoCommandHandler : IRequestHandler<RealizarAutenticacaoCommand, string>
{
    private readonly IUsuarioRepository _usuarioRepository;

    private readonly ICriptografarSenhaService _criptografarSenhaService;

    private readonly IGerarTokenJwtService _gerarTokenJwtService;

    public RealizarAutenticacaoCommandHandler(IUsuarioRepository usuarioRepository,
        IGerarTokenJwtService gerarTokenJwtService, ICriptografarSenhaService criptografarSenhaService)
    {
        _usuarioRepository = usuarioRepository;
        _gerarTokenJwtService = gerarTokenJwtService;
        _criptografarSenhaService = criptografarSenhaService;
    }

    public async Task<string> Handle(RealizarAutenticacaoCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorNomeAsync(request.Nome, cancellationToken);

        if (usuario is null)
            throw new ValidacaoException(["Usuário ou senha inválidos."], HttpStatusCode.Unauthorized);

        var senhaValida = _criptografarSenhaService.Verificar(request.Senha, usuario.Senha);

        if (!senhaValida)
            throw new ValidacaoException(["Usuário ou senha inválidos."], HttpStatusCode.Unauthorized);

        return _gerarTokenJwtService.Gerar(usuario.Id, usuario.Nome);
    }
}