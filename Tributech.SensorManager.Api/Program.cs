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

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1); // Default to version 1.0
            options.ReportApiVersions = true;
        }).AddApiExplorer(o =>
        {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<SensorTypeSchemaFilter>();
            c.SchemaFilter<SensorMetadataVmSchemaFilter>();

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.DocInclusionPredicate((version, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v}" == version);
            });
        });

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