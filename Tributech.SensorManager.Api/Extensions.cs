using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

using Tributech.SensorManager.Application;

namespace Tributech.SensorManager.Api;

public static class Extensions
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

            options.JsonSerializerOptions.ConfigureApplicationJsonOptions();

            // Add more configuration options as needed

            setupAction?.Invoke(options);
        });

        return builder;
    }
}