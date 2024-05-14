using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Tributech.SensorManager.Application.MandatoryMetadatas.Commands;
using Tributech.SensorManager.Application.MandatoryMetadatas.Common;
using Tributech.SensorManager.Application.MandatoryMetadatas.Queries;
using Tributech.SensorManager.Domain.ValueObjects;

namespace IntegrationTests;

// Mandatory metadata CRUD tests
public class MandatoryMetadataTests
{
    [Fact]
    public async Task CreateMandatoryMetadata()
    {
        using var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var mediatr = sp.GetRequiredService<IMediator>();
        Assert.NotNull(mediatr);

        var createCommand = new CreateMandatoryMetadataCommand
        {
            SensorType = "TestType",
            Metadata = new List<MandatoryMetadataItemVm>
            {
                new() { Key = "Location", Type = FieldType.String }
            }
        };

        var mandatoryMetadata = await mediatr.Send(createCommand);
        Assert.NotNull(mandatoryMetadata);
        Assert.Equal("TestType", mandatoryMetadata.SensorType);
        Assert.Single(mandatoryMetadata.Metadata);
        Assert.Equal("Location", mandatoryMetadata.Metadata.First().Key);
    }

    [Fact]
    public async Task UpdateMandatoryMetadata()
    {
        using var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var mediatr = sp.GetRequiredService<IMediator>();
        Assert.NotNull(mediatr);

        var createCommand = new CreateMandatoryMetadataCommand
        {
            SensorType = "TestType",
            Metadata = new List<MandatoryMetadataItemVm>
            {
                new() { Key = "Location", Type = FieldType.String }
            }
        };

        var mandatoryMetadata = await mediatr.Send(createCommand);
        Assert.NotNull(mandatoryMetadata);
        Assert.Single(mandatoryMetadata.Metadata);
        Assert.Equal("Location", mandatoryMetadata.Metadata.First().Key);

        var updateCommand = new UpdateMandatoryMetadataCommand
        {
            Id = mandatoryMetadata.Id,
            Type = "TestType",
            Metadata = new List<MandatoryMetadataItemVm>
            {
                new() { Key = "Location", Type = FieldType.String, DefaultValue = "Living Room" }
            }
        };

        mandatoryMetadata = await mediatr.Send(updateCommand);
        Assert.NotNull(mandatoryMetadata);
        Assert.Single(mandatoryMetadata.Metadata);
        Assert.Equal("Location", mandatoryMetadata.Metadata.First().Key);
        Assert.Equal("Living Room", mandatoryMetadata.Metadata.First().DefaultValue);
    }

    [Fact]
    public async Task DeleteMandatoryMetadata()
    {
        using var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var mediatr = sp.GetRequiredService<IMediator>();
        Assert.NotNull(mediatr);

        var createCommand = new CreateMandatoryMetadataCommand
        {
            SensorType = "TestType",
            Metadata = new List<MandatoryMetadataItemVm>
            {
                new() { Key = "Location", Type = FieldType.String }
            }
        };

        var mandatoryMetadata = await mediatr.Send(createCommand);
        Assert.NotNull(mandatoryMetadata);
        Assert.Single(mandatoryMetadata.Metadata);
        Assert.Equal("Location", mandatoryMetadata.Metadata.First().Key);

        var deleteCommand = new DeleteMandatoryMetadataCommand
        {
            Id = mandatoryMetadata.Id
        };

        await mediatr.Send(deleteCommand);

        mandatoryMetadata = await mediatr.Send(new GetMandatoryMetadataQuery { SensorType = "TestType" });
        Assert.Null(mandatoryMetadata);
    }

    private static ServiceProvider CreateServiceProvider()
        => IntegrationTestHelper.CreateServiceProvider();
}