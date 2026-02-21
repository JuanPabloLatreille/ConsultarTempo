using Application.Autenticacao.CriarCadastro;
using Application.Autenticacao.RealizarAutenticacao;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("autenticar")]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar([FromBody] RealizarAutenticacaoCommand command)
    {
        var token = await _mediator.Send(command);

        return Ok(new { token });
    }

    [HttpPost("criar-cadastro")]
    [AllowAnonymous]
    public async Task<IActionResult> CriarCadastro([FromBody] CriarCadastroCommand command)
    {
        await _mediator.Send(command);

        return Created(string.Empty, new { mensagem = "Cadastro realizado com sucesso!" });
    }
}