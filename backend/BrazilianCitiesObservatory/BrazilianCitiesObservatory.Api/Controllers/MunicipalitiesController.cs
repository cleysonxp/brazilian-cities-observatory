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
    public async Task<ActionResult<PagedResultDto<Municipality>>> Get(
        [FromQuery] int? stateId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Municipalities
            .AsNoTracking()
            .AsQueryable();

        if (stateId.HasValue)
        {
            query = query.Where(
                municipality => municipality.StateId == stateId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var municipalities = await query
            .OrderBy(municipality => municipality.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new PagedResultDto<Municipality>
        {
            Items = municipalities,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };

        return Ok(result);
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