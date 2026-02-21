using Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperatura;
using Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperaturaPorCoordenadas;
using Application.HistoricosTemperaturas.Commands.RemoverHistoricoTemperatura;
using Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorCoordenadas;
using Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorNomeCidade;
using Application.HistoricosTemperaturas.Queries.ConsultarTodosHistoricos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoricosTemperaturasController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistoricosTemperaturasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(new ConsultarTodosHistoricosQuery(), cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("por-nome/{nomeCidade}")]
    public async Task<IActionResult> ConsultarPorNome(
        string nomeCidade, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(
            new ConsultarHistoricoPorNomeCidadeQuery(nomeCidade), cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("por-coordenadas")]
    public async Task<IActionResult> ConsultarPorCoordenadas(
        [FromQuery] double latitude, [FromQuery] double longitude, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(
            new ConsultarHistoricoPorCoordenadasQuery(latitude, longitude), cancellationToken);
        return Ok(resultado);
    }

    [HttpPost("por-nome")]
    public async Task<IActionResult> CadastrarPorNome(
        [FromBody] CadastrarHistoricoTemperaturaCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, resultado);
    }

    [HttpPost("por-coordenadas")]
    public async Task<IActionResult> CadastrarPorCoordenadas(
        [FromBody] CadastrarHistoricoTemperaturaPorCoordenadasCommand command, CancellationToken cancellationToken)
    {
        var resultado = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, resultado);
    }

    [HttpDelete("id/{id:guid}")]
    public async Task<IActionResult> RemoverPorId(
        Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoverHistoricoTemperaturaCommand(id), cancellationToken);
        return NoContent();
    }
}