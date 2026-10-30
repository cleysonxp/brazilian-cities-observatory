using BrazilianCitiesObservatory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BrazilianCitiesObservatory.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<State> States { get; set; }

    public DbSet<Municipality> Municipalities { get; set; }
}