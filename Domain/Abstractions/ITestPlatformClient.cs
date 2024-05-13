namespace Tributech.SensorManager.Domain.Abstractions;

public interface ITestPlatformClient
{
    Task<SensorValuesResponse> GetSensorValuesAsync(string sensorId, string from, string to);
}