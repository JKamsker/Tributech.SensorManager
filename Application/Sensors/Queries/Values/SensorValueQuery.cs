using MediatR;

using Tributech.SensorManager.Library.External.TestPlatform;

namespace Tributech.SensorManager.Application.Sensors.Queries.Values;

public class SensorValuesRequest : IRequest<SensorValuesResponse>
{
    public string SensorId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}

// response
public class SensorValuesResponse
{
    public string StreamId { get; set; }
    public string Timestamp { get; set; }
    public string StoredAt { get; set; }
    public int Value { get; set; }

    public SensorValuesResponse(Tributech.SensorManager.Library.External.TestPlatform.SensorValuesResponse sensorValuesResponse)
    {
        StreamId = sensorValuesResponse.StreamId;
        Timestamp = sensorValuesResponse.Timestamp;
        StoredAt = sensorValuesResponse.StoredAt;
        Value = sensorValuesResponse.Value;
    }
}

public class SensorValuesHandler : IRequestHandler<SensorValuesRequest, SensorValuesResponse>
{
    private readonly ITestPlatformClient _testPlatformClient;

    public SensorValuesHandler(ITestPlatformClient testPlatformClient)
    {
        _testPlatformClient = testPlatformClient;
    }

    public async Task<SensorValuesResponse> Handle(SensorValuesRequest request, CancellationToken cancellationToken)
    {
        var result = await _testPlatformClient.GetSensorValuesAsync(request.SensorId, request.From, request.To);
        return new SensorValuesResponse(result);
    }
}