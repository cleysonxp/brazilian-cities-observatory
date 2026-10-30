using System.Text.Json.Serialization;

namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class IbgeStateDto
{
    public int Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("sigla")]
    public string Sigla { get; set; } = string.Empty;
}