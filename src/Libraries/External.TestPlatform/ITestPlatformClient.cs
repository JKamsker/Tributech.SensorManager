using Tributech.SensorManager.Library.External.TestPlatform.Models;

namespace Tributech.SensorManager.Library.External.TestPlatform;

public interface ITestPlatformClient
{
    IAsyncEnumerable<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to);
}