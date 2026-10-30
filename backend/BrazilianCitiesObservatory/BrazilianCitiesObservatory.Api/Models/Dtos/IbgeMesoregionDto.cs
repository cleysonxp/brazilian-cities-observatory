using System.Text.Json.Serialization;

namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class IbgeMesoregionDto
{
    [JsonPropertyName("UF")]
    public IbgeStateDto? UF { get; set; }
}