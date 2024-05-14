﻿using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Tributech.SensorManager.Application.MandatoryMetadatas.Commands;
using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Application.Sensors.Commands;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries;
using Tributech.SensorManager.Domain.ValueObjects;

namespace IntegrationTests;

// sensor crud tests
public class SensorTests
{
    [Fact]
    public async Task CreateSensor()
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

    [Fact]
    public async Task UpdateSensor()
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

        var updateCommand = new UpdateSensorCommand
        {
            Id = sensor.Id,
            Name = "UpdatedSensor",
        };

        sensor = await mediatr.Send(updateCommand);
        Assert.NotNull(sensor);
        Assert.Equal("UpdatedSensor", sensor.Name);

        var getCommand = new GetSensorQuery { Id = sensor.Id };
        sensor = await mediatr.Send(getCommand);

        Assert.NotNull(sensor);
        Assert.Equal("UpdatedSensor", sensor.Name);

        Assert.Single(sensor.Metadata);
        Assert.Equal("Location", sensor.Metadata.First().Key);
        Assert.Equal("Living Room", sensor.Metadata.First().Value);
    }

    [Fact]
    public async Task DeleteSensor()
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

        var deleteCommand = new DeleteSensorCommand
        {
            Id = sensor.Id
        };

        await mediatr.Send(deleteCommand);

        sensor = await mediatr.Send(new GetSensorQuery { Id = sensor.Id });
        Assert.Null(sensor);
    }

    private static ServiceProvider CreateServiceProvider()
        => IntegrationTestHelper.CreateServiceProvider();
}