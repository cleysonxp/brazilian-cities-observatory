namespace BrazilianCitiesObservatory.Api.Models;

public class State
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Acronym { get; set; } = string.Empty;

    public ICollection<Municipality> Municipalities { get; set; } = [];
}