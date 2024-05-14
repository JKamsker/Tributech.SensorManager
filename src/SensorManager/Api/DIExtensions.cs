using Asp.Versioning;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

using Tributech.SensorManager.Api.SchemaFilters;
using Tributech.SensorManager.Application.Extensions;
using Tributech.SensorManager.Domain.Extensions;

namespace Tributech.SensorManager.Api;

public static class DIExtensions
{
    // MvcBuilder addjsonoptions
    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder, Action<JsonOptions>? setupAction = null)
    {
        //builder.Services.Configure(setupAction);
        builder.AddJsonOptions(options =>
        {
            // Configure the JSON serializer options here
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.IgnoreNullValues = true;

            options.JsonSerializerOptions
                .ConfigureApplicationJsonOptions()
                .ConfigureDomainJsonOptions()
                ;

            // Add more configuration options as needed

            setupAction?.Invoke(options);
        });

        return builder;
    }

    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Authentication:Authority"];
        if (string.IsNullOrWhiteSpace(authority))
        {
            throw new InvalidOperationException("Keycloak authority is not configured, please provide the authority in the appsettings.json file under the Authentication:Authority key");
        }

        var debugMode = configuration.GetValue<bool>("Authentication:DebugMode", false);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // This automatically retreives the public key from the Keycloak server so we do not need to provide it manually
            //options.Authority = "http://localhost:8085/realms/customer";

            options.Authority = authority;

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                // Set clock skew to zero if you want exact token expiration timing, optional
                //ClockSkew = TimeSpan.Zero,
                ValidateLifetime = !debugMode,
            };
        });

        return services;
    }

    public static void UseKeycloakAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static IServiceCollection AddOpenApiIntegration(this IServiceCollection services)
    {
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

        return services;
    }
}