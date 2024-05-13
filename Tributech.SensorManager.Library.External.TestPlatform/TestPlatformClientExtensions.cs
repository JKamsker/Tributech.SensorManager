using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Polly;
using Tributech.SensorManager.Library.External.TestPlatform.Models;

namespace Tributech.SensorManager.Library.External.TestPlatform;

public static class TestPlatformClientExtensions
{
    public static IServiceCollection AddTestPlatformClient(this IServiceCollection services, IConfiguration configuration)
    {
        var clientOptions = configuration.GetSection("TestPlatformClientOptions").Get<TestPlatformClientOptions>() ?? new TestPlatformClientOptions
        {
            ClientType = "Static"
        };

        if (clientOptions.ClientType == "Static")
        {
            services.AddSingleton<ITestPlatformClient, StaticTestPlatformClient>();
        }
        else if (clientOptions.ClientType == "Http")
        {
            if (string.IsNullOrWhiteSpace(clientOptions.BaseUrl))
            {
                throw new ArgumentException("BaseUrl must be provided for Http client type.");
            }

            services.AddHttpClient<ITestPlatformClient, TestPlatformClient>(client =>
            {
                client.BaseAddress = new Uri(clientOptions.BaseUrl);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, retryAttempt => Backoff(retryAttempt)));
        }

        return services;
    }

    private static TimeSpan Backoff(int retryAttempt)
    {
        var waitTimeInSeconds = Math.Pow(2, retryAttempt);

        // + 0 to 20% random factor
        var jitter = Random.Shared.NextDouble() * waitTimeInSeconds * 0.2;

        return TimeSpan.FromSeconds(jitter);
    }
}
