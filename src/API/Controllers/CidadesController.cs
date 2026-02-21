using Application.Cidades.Commands.CadastrarCidade;
using Application.Cidades.Queries.ConsultarCidadePorId;
using Application.Cidades.Queries.ConsultarTodasCidades;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CidadesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CidadesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(
        Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ConsultarCidadePorIdQuery(id), cancellationToken);
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas(CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ConsultarTodasCidadesQuery(), cancellationToken);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(
        [FromBody] CadastrarCidadeCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, resultado);
    }
}