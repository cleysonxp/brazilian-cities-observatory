using System.Text.Json.Serialization;

namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class IbgeMicroregionDto
{
    [JsonPropertyName("mesorregiao")]
    public IbgeMesoregionDto? Mesorregiao { get; set; }
}