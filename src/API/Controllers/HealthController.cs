using Infraestructure.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationContext _context;

    public HealthController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Check()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            return Ok(new { status = "Healthy" });
        }
        catch
        {
            return StatusCode(503, new { status = "Unhealthy" });
        }
    }
}