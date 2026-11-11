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
    public async Task<ActionResult<PagedResultDto<MunicipalityResponseDto>>> Get(
        [FromQuery] int? stateId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
        {
            return BadRequest("O número da página deve ser maior ou igual a 1.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("O tamanho da página deve estar entre 1 e 100.");
        }

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
            .ThenBy(municipality => municipality.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(municipality => new MunicipalityResponseDto
            {
                Id = municipality.Id,
                Name = municipality.Name,
                State = new StateResponseDto
                {
                    Id = municipality.State.Id,
                    Name = municipality.State.Name,
                    Acronym = municipality.State.Acronym
                }
            })
            .ToListAsync(cancellationToken);

        var result = new PagedResultDto<MunicipalityResponseDto>
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