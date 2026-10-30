using BrazilianCitiesObservatory.Api.Models.Dtos;
using BrazilianCitiesObservatory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrazilianCitiesObservatory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MunicipalitiesController : ControllerBase
{
    private readonly IbgeService _ibgeService;
    private readonly IbgeImportService _ibgeImportService;

    public MunicipalitiesController(
        IbgeService ibgeService,
        IbgeImportService ibgeImportService)
    {
        _ibgeService = ibgeService;
        _ibgeImportService = ibgeImportService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var result = await _ibgeService.GetImportDataAsync(
            cancellationToken);

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