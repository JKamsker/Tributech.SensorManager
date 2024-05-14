using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Tributech.SensorManager.Application.MandatoryMetadatas.Commands;
using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Application.Sensors.Commands;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries;
using Tributech.SensorManager.Domain.ValueObjects;

namespace IntegrationTests;

// caching tests: Creates a sensor, lists all, creates another sensor, lists all again, updates the first sensor, lists all again, deletes the first sensor, lists all again
public class CachingTests
{
    [Fact]
    public async Task CacheTests()
    {
        using var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var mediatr = sp.GetRequiredService<IMediator>();
        Assert.NotNull(mediatr);

        var createMandatoryMetadataCommand = new CreateMandatoryMetadataCommand
        {
            SensorType = "TestType",
            Metadata = new List<MandatoryMetadataItemVm>
            {
                new() { Key = "Location", Type = FieldType.String }
            }
        };

        await mediatr.Send(createMandatoryMetadataCommand);

        var createCommand = new CreateSensorCommand
        {
            Type = "TestType",
            Name = "TestSensor",
            Metadata = new List<SensorMetadataVm>
            {
                new() { Key = "Location", Value = "Living Room" }
            }
        };

        var sensor = await mediatr.Send(createCommand);
        Assert.NotNull(sensor);
        Assert.Equal("TestSensor", sensor.Name);

        var allSensors = await mediatr.Send(new GetAllSensorsQuery());
        Assert.Single(allSensors);

        var createCommand2 = new CreateSensorCommand
        {
            Type = "TestType",
            Name = "TestSensor2",
            Metadata = new List<SensorMetadataVm>
            {
                new() { Key = "Location", Value = "Living Room" }
            }
        };

        var sensor2 = await mediatr.Send(createCommand2);
        Assert.NotNull(sensor2);
        Assert.Equal("TestSensor2", sensor2.Name);

        allSensors = await mediatr.Send(new GetAllSensorsQuery());
        Assert.Equal(2, allSensors.Count);

        var updateCommand = new UpdateSensorCommand
        {
            Id = sensor.Id,
            Name = "UpdatedSensor",
        };

        sensor = await mediatr.Send(updateCommand);
        Assert.NotNull(sensor);
        Assert.Equal("UpdatedSensor", sensor.Name);

        allSensors = await mediatr.Send(new GetAllSensorsQuery());
        Assert.Equal(2, allSensors.Count);

        var deleteCommand = new DeleteSensorCommand
        {
            Id = sensor.Id
        };

        await mediatr.Send(deleteCommand);

        allSensors = await mediatr.Send(new GetAllSensorsQuery());
        Assert.Single(allSensors);
    }

    private static ServiceProvider CreateServiceProvider()
        => IntegrationTestHelper.CreateServiceProvider();
}