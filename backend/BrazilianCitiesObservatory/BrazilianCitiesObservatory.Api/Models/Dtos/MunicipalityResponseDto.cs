namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class MunicipalityResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public StateResponseDto State { get; set; } = null!;
}