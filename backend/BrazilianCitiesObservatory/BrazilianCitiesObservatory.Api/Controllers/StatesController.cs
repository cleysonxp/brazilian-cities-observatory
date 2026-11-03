using BrazilianCitiesObservatory.Api.Data;
using BrazilianCitiesObservatory.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrazilianCitiesObservatory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StatesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<State>>> Get(
        CancellationToken cancellationToken)
    {
        var states = await _context.States
            .AsNoTracking()
            .OrderBy(state => state.Name)
            .ToListAsync(cancellationToken);

        return Ok(states);
    }
}