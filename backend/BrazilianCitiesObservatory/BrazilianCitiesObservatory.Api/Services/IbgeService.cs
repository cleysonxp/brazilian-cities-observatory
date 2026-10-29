using System.Net.Http.Json;
using BrazilianCitiesObservatory.Api.Models;

namespace BrazilianCitiesObservatory.Api.Services;

public class IbgeService
{
    private readonly HttpClient _httpClient;

    public IbgeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Municipality>> GetMunicipalitiesAsync(
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

        return ibgeMunicipalities
            .Select(municipality => new Municipality
            {
                Id = municipality.Id,
                Name = municipality.Nome
            })
            .ToList();
    }

    private class IbgeMunicipalityDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
    }
}