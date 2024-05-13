using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Polly;

using System.Text.Json;

namespace Tributech.SensorManager.Library.External.TestPlatform;

public interface ITestPlatformClient
{
    Task<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to);
}

public class SensorValuesResponse
{
    public string StreamId { get; set; }
    public string Timestamp { get; set; }
    public string StoredAt { get; set; }
    public int Value { get; set; }
}

public class StaticTestPlatformClient : ITestPlatformClient
{
    public Task<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to)
    {
        var staticResponse = new SensorValuesResponse
        {
            StreamId = sensorId,
            Timestamp = "2020-01-01T00:00:00.000+00:00",
            StoredAt = "2020-01-01T00:00:00.000+00:00",
            Value = 0
        };

        return Task.FromResult(staticResponse);
    }
}

public class TestPlatformClient : ITestPlatformClient
{
    private readonly HttpClient _httpClient;

    public TestPlatformClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to)
    {
        var url = $"/values/double?StreamId={sensorId}&From={from}&To={to}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var sensorValues = JsonSerializer.Deserialize<SensorValuesResponse>(content);

        return sensorValues;
    }
}

// di
public static class TestPlatformClientExtensions
{
    // same with polly
    //public static IServiceCollection AddTestPlatformClient(this IServiceCollection services)
    //{
    //    services.AddHttpClient<ITestPlatformClient, TestPlatformClient>(client =>
    //    {
    //        client.BaseAddress = new Uri("https://testplatform.io");
    //    })
    //    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, retryAttempt => Backoff(retryAttempt)));

    //    return services;
    //}

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

public class TestPlatformClientOptions
{
    public string ClientType { get; set; }
    public string BaseUrl { get; set; }
}