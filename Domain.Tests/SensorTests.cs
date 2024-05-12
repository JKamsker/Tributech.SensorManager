using System.ComponentModel.DataAnnotations;

using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Domain.Tests;

public class SensorTests
{
    [Fact]
    public void CheckMandatoryMetadata_ShouldThrowException_WhenMandatoryMetadataIsNotSet()
    {
        // Arrange
        var sensor = new Sensor();
        var mandatoryMetadataItems = new List<MandatoryMetadataItem>
        {
            new MandatoryMetadataItem { Key = "mandatoryKey" }
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => sensor.CheckMandatoryMetadata(mandatoryMetadataItems));
    }

    [Fact]
    public void CheckMandatoryMetadata_ShouldSetDefaultValue_WhenMandatoryMetadataIsNotSetAndDefaultValueExists()
    {
        // Arrange
        var sensor = new Sensor();
        var mandatoryKey = "mandatoryKey";
        var defaultValue = "default";
        var mandatoryMetadataItems = new List<MandatoryMetadataItem>
        {
            new MandatoryMetadataItem { Key = mandatoryKey, DefaultValue = defaultValue }
        };

        // Act
        sensor.CheckMandatoryMetadata(mandatoryMetadataItems);

        // Assert
        Assert.Single(sensor.Metadata);
        Assert.Equal(mandatoryKey, sensor.Metadata.First().Key);
        Assert.Equal(defaultValue, sensor.Metadata.First().Value);
    }

    // Test type check
    [Fact]
    public void CheckMandatoryMetadata_ShouldThrowException_WhenMandatoryMetadataIsNotSetAndTypeIsNotMatching()
    {
        // Arrange
        var sensor = new Sensor
        {
            Type = SensorType.Temperature,
            Metadata = new List<SensorMetadata>
            {
                new SensorMetadata { Key = "mandatoryKey", Value = "value" }
            }
        };
        var mandatoryMetadataItems = new List<MandatoryMetadataItem>
        {
            new MandatoryMetadataItem { Key = "mandatoryKey", Type = ValueObjects.ValueType.Int }
        };

        // Act & Assert
        Assert.Throws<ValidationException>(() => sensor.CheckMandatoryMetadata(mandatoryMetadataItems));
    }

    // Typecheck success
    [Fact]
    public void CheckMandatoryMetadata_ShouldNotThrowException_WhenMandatoryMetadataIsSetAndTypeIsMatching()
    {
        // Arrange
        var sensor = new Sensor
        {
            Type = SensorType.Temperature,
            Metadata = new List<SensorMetadata>
            {
                new SensorMetadata { Key = "mandatoryKey", Value = "1" }
            }
        };
        var mandatoryMetadataItems = new List<MandatoryMetadataItem>
        {
            new MandatoryMetadataItem { Key = "mandatoryKey", Type = ValueObjects.ValueType.Int }
        };

        // Act & Assert
        sensor.CheckMandatoryMetadata(mandatoryMetadataItems);
    }
}