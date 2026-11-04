using BrazilianCitiesObservatory.Api.Data;
using BrazilianCitiesObservatory.Api.Models;
using BrazilianCitiesObservatory.Api.Models.Dtos;
using BrazilianCitiesObservatory.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrazilianCitiesObservatory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MunicipalitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IbgeImportService _ibgeImportService;

    public MunicipalitiesController(
        ApplicationDbContext context,
        IbgeImportService ibgeImportService)
    {
        _context = context;
        _ibgeImportService = ibgeImportService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Municipality>>> Get(
        [FromQuery] int? stateId,
        CancellationToken cancellationToken)
    {
        var query = _context.Municipalities
            .AsNoTracking()
            .AsQueryable();

        if (stateId.HasValue)
        {
            query = query.Where(
                municipality => municipality.StateId == stateId.Value);
        }

        var municipalities = await query
            .OrderBy(municipality => municipality.Name)
            .ToListAsync(cancellationToken);

        return Ok(municipalities);
    }

    [HttpPost("import")]
    public async Task<ActionResult<IbgeImportSummary>> Import(
        CancellationToken cancellationToken)
    {
        var result = await _ibgeImportService.ImportAsync(
            cancellationToken);

        return Ok(result);
    }
}