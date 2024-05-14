using System.Net.Http.Json;

using Tributech.SensorManager.Library.External.TestPlatform.Models;

namespace Tributech.SensorManager.Library.External.TestPlatform;

internal class TestPlatformClient(HttpClient _httpClient) : ITestPlatformClient
{
    public async IAsyncEnumerable<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to)
    {
        var url = $"/values/double?StreamId={sensorId}&From={from}&To={to}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var result = response.Content.ReadFromJsonAsAsyncEnumerable<SensorValuesResponse>();
        await foreach (var sensorValue in result)
        {
            yield return sensorValue;
        }
    }
}