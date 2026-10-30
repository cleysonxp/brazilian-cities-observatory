using BrazilianCitiesObservatory.Api.Data;
using BrazilianCitiesObservatory.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace BrazilianCitiesObservatory.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddHttpClient<IbgeService>(client =>
            {
                client.BaseAddress = new Uri(
                    "https://servicodados.ibge.gov.br/api/v1/localidades/");
            });

            builder.Services.AddScoped<IbgeImportService>();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}