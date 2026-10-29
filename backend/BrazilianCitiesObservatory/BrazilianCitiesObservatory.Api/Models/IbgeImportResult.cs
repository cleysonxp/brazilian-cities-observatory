namespace BrazilianCitiesObservatory.Api.Models;

public class IbgeImportResult
{
    public List<State> States { get; set; } = [];

    public List<Municipality> Municipalities { get; set; } = [];
}