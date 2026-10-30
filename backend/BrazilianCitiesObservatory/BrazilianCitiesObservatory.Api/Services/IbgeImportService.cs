using BrazilianCitiesObservatory.Api.Data;
using BrazilianCitiesObservatory.Api.Models;
using BrazilianCitiesObservatory.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BrazilianCitiesObservatory.Api.Services;

public class IbgeImportService
{
    private readonly IbgeService _ibgeService;
    private readonly ApplicationDbContext _context;

    public IbgeImportService(
        IbgeService ibgeService,
        ApplicationDbContext context)
    {
        _ibgeService = ibgeService;
        _context = context;
    }

    public async Task<IbgeImportSummary> ImportAsync(
        CancellationToken cancellationToken = default)
    {
        var importData = await _ibgeService.GetImportDataAsync(
            cancellationToken);

        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        var statesInserted = 0;
        var statesUpdated = 0;
        var municipalitiesInserted = 0;
        var municipalitiesUpdated = 0;

        var existingStates = await _context.States
            .ToDictionaryAsync(state => state.Id, cancellationToken);

        foreach (var state in importData.States)
        {
            if (existingStates.TryGetValue(state.Id, out var existingState))
            {
                existingState.Name = state.Name;
                existingState.Acronym = state.Acronym;

                statesUpdated++;
            }
            else
            {
                _context.States.Add(state);
                statesInserted++;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var existingMunicipalities = await _context.Municipalities
            .ToDictionaryAsync(municipality => municipality.Id, cancellationToken);

        foreach (var municipality in importData.Municipalities)
        {
            if (existingMunicipalities.TryGetValue(
                municipality.Id, out var existingMunicipality))
            {
                existingMunicipality.Name = municipality.Name;
                existingMunicipality.StateId = municipality.StateId;

                municipalitiesUpdated++;
            }
            else
            {
                _context.Municipalities.Add(municipality);
                municipalitiesInserted++;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new IbgeImportSummary
        {
            StatesInserted = statesInserted,
            StatesUpdated = statesUpdated,
            MunicipalitiesInserted = municipalitiesInserted,
            MunicipalitiesUpdated = municipalitiesUpdated
        };
    }
}