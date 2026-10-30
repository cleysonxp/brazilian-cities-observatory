using System.Text.Json.Serialization;

namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class IbgeMunicipalityDto
{
    public int Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("microrregiao")]
    public IbgeMicroregionDto? Microrregiao { get; set; }
}