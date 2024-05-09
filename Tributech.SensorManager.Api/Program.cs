using System.Text.Json;

using Tributech.SensorManager.Application;
using Tributech.SensorManager.Infrastructure;

namespace Tributech.SensorManager.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                // Configure the JSON serializer options here
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;

                options.JsonSerializerOptions.ConfigureJsonOptions();

                // Add more configuration options as needed
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddMediatR(builder => builder.RegisterServicesFromAssemblyContaining<ApplicationStub>());

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