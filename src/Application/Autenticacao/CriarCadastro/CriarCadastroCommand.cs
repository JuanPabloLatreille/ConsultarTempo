using System.Net;
using Domain.Entities.Usuarios;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Domain.Interfaces.Usuarios;
using MediatR;

namespace Application.Autenticacao.CriarCadastro;

public sealed record CriarCadastroCommand(string Nome, string Senha) : IRequest;

public sealed class CriarCadastroCommandHandler : IRequestHandler<CriarCadastroCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    private readonly ICriptografarSenhaService _criptografarSenhaService;

    private readonly IUnitOfWork _unitOfWork;

    public CriarCadastroCommandHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork,
        ICriptografarSenhaService criptografarSenhaService)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _criptografarSenhaService = criptografarSenhaService;
    }

    public async Task Handle(CriarCadastroCommand request, CancellationToken cancellationToken)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorNomeAsync(request.Nome, cancellationToken);

        if (usuarioExistente is not null)
            throw new ValidacaoException(["Já existe um usuário com esse nome."], HttpStatusCode.Conflict);

        var senhaHash = _criptografarSenhaService.Criptografar(request.Senha);

        var usuario = Usuario.Criar(request.Nome, senhaHash);

        await _usuarioRepository.AdicionarAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}