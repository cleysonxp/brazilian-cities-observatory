using BrazilianCitiesObservatory.Api.Models;

namespace BrazilianCitiesObservatory.Api.Models.Dtos;

public class IbgeImportResult
{
    public List<State> States { get; set; } = [];

    public List<Municipality> Municipalities { get; set; } = [];
}