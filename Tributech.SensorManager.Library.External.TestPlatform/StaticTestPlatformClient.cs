using Tributech.SensorManager.Library.External.TestPlatform.Models;

namespace Tributech.SensorManager.Library.External.TestPlatform;

public class StaticTestPlatformClient : ITestPlatformClient
{
    public IAsyncEnumerable<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to)
    {
        var staticResponse = new SensorValuesResponse
        {
            StreamId = sensorId,
            Timestamp = "2020-01-01T00:00:00.000+00:00",
            StoredAt = "2020-01-01T00:00:00.000+00:00",
            Value = 0
        };

        return new List<SensorValuesResponse> { staticResponse }.ToAsyncEnumerable();
    }
}
