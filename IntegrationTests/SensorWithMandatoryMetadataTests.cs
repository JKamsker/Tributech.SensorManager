using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel.DataAnnotations;

using Tributech.SensorManager.Application.MandatoryMetadatas.Commands;
using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Application.Sensors.Commands;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Domain.ValueObjects;

namespace IntegrationTests;

public class SensorWithMandatoryMetadataTests
{
    [Fact]
    public async Task CreateSensor_WithMetadataPresent()
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
    }

    // Same, but with mandatory metadata missing
    [Fact]
    public async Task CreateSensor_WithMissingMandatoryMetadata()
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
                new() { Key = "Manufacturer", Value = "Test Inc." }
            }
        };

        await Assert.ThrowsAsync<ValidationException>(() => mediatr.Send(createCommand));
    }

    // Create mandatory metadata with default value, create sensor without metadata, metadata should be set to default value
    [Fact]
    public async Task CreateSensor_WithDefaultMandatoryMetadata()
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
                new() { Key = "Location", Type = FieldType.String, DefaultValue = "Living Room" }
            }
        };

        await mediatr.Send(createMandatoryMetadataCommand);

        var createCommand = new CreateSensorCommand
        {
            Type = "TestType",
            Name = "TestSensor"
        };

        var sensor = await mediatr.Send(createCommand);
        Assert.NotNull(sensor);
        Assert.Equal("TestSensor", sensor.Name);
        Assert.Single(sensor.Metadata);
        Assert.Equal("Location", sensor.Metadata.First().Key);
        Assert.Equal("Living Room", sensor.Metadata.First().Value);
    }

    private static ServiceProvider CreateServiceProvider()
        => IntegrationTestHelper.CreateServiceProvider();
}