using Asp.Versioning;

using FluentValidation;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Globalization;
using System.Reflection;

using Tributech.SensorManager.Api.SchemaFilters;
using Tributech.SensorManager.Application;
using Tributech.SensorManager.Application.Behavior;
using Tributech.SensorManager.Infrastructure;
using Tributech.SensorManager.Library.External.TestPlatform;

namespace Tributech.SensorManager.Api;

public class Program
{
    public static void Main(string[] args)
    {
        // Set threadlanguage to english
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseKeycloakAuth();

        app.MapControllers();

        app.Run();
    }

    internal static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllers()
            .ConfigureJsonOptions();

        services.AddOpenApiIntegration();

        services.AddInfrastructureServices(configuration);
        services.AddMediatR(builder =>
        {
            builder.RegisterServicesFromAssemblyContaining<ApplicationStub>();
            builder.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationStub).Assembly);

        services.AddMemoryCache();

        services.AddProblemDetails();
        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddKeycloakAuthentication(configuration);

        services.AddTestPlatformClient(configuration);
    }
}