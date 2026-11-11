namespace BrazilianCitiesObservatory.Api.Models;

public class Municipality
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int StateId { get; set; }

    public State State { get; set; } = null!;
}