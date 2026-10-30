
using System.Net.Http.Json;
using BrazilianCitiesObservatory.Api.Models;
using BrazilianCitiesObservatory.Api.Models.Dtos;

namespace BrazilianCitiesObservatory.Api.Services;

public class IbgeService
{
    private readonly HttpClient _httpClient;

    public IbgeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IbgeImportResult> GetImportDataAsync(
        CancellationToken cancellationToken = default)
    {
        var ibgeMunicipalities =
            await _httpClient.GetFromJsonAsync<List<IbgeMunicipalityDto>>(
                "municipios",
                cancellationToken);

        if (ibgeMunicipalities is null)
        {
            throw new InvalidOperationException(
                "A API do IBGE não retornou uma lista de municípios.");
        }

        var statesById = new Dictionary<int, State>();
        var municipalities = new List<Municipality>();

        foreach (var ibgeMunicipality in ibgeMunicipalities)
        {
            var ibgeState = ibgeMunicipality
                .Microrregiao?
                .Mesorregiao?
                .UF;

            if (ibgeState is null)
            {
                continue;
            }

            if (!statesById.ContainsKey(ibgeState.Id))
            {
                statesById.Add(
                    ibgeState.Id,
                    new State
                    {
                        Id = ibgeState.Id,
                        Name = ibgeState.Nome,
                        Acronym = ibgeState.Sigla
                    });
            }

            municipalities.Add(new Municipality
            {
                Id = ibgeMunicipality.Id,
                Name = ibgeMunicipality.Nome,
                StateId = ibgeState.Id
            });
        }

        return new IbgeImportResult
        {
            States = statesById.Values.ToList(),
            Municipalities = municipalities
        };
    }
}
