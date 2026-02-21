using Domain.Entities.Cidades;
using Domain.Interfaces;
using Domain.Interfaces.Cidades;
using MediatR;

namespace Application.Cidades.Commands.CadastrarCidade;

public sealed record CadastrarCidadeCommand(
    string Nome,
    string Uf,
    double Latitude,
    double Longitude) : IRequest<CadastrarCidadeResult>;

public sealed class CadastrarCidadeCommandHandler : IRequestHandler<CadastrarCidadeCommand, CadastrarCidadeResult>
{
    private readonly ICidadeRepository _cidadeRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CadastrarCidadeCommandHandler(ICidadeRepository cidadeRepository, IUnitOfWork unitOfWork)
    {
        _cidadeRepository = cidadeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarCidadeResult> Handle(CadastrarCidadeCommand request, CancellationToken cancellationToken)
    {
        var cidade = Cidade.Criar(request.Nome, request.Uf, request.Latitude, request.Longitude);

        await _cidadeRepository.AdicionarAsync(cidade, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CadastrarCidadeResult(
            cidade.Id,
            cidade.Nome,
            cidade.Uf,
            cidade.Latitude,
            cidade.Longitude,
            cidade.CriadoEm);
    }
}