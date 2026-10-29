using BrazilianCitiesObservatory.Api.Models;
using BrazilianCitiesObservatory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrazilianCitiesObservatory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MunicipalitiesController : ControllerBase
{
    private readonly IbgeService _ibgeService;

    public MunicipalitiesController(IbgeService ibgeService)
    {
        _ibgeService = ibgeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Municipality>>> Get(
        CancellationToken cancellationToken)
    {
        var municipalities = await _ibgeService.GetMunicipalitiesAsync(
            cancellationToken);

        return Ok(municipalities);
    }
}