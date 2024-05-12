using System.Text.Json;
using Tributech.SensorManager.Application.Sensors.Common;
using Tributech.SensorManager.Application.Sensors.Queries.Common;
using Tributech.SensorManager.Domain.Entities;

namespace Application.Tests;

public class SensorVM_SerializationTest
{
    [Fact]
    public void Serialize_SensorVm_CorrectJsonOutput()
    {
        // Arrange
        var sensor = new SensorVm
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789abc"),
            Name = "Example Sensor",
            Metadata = new List<SensorMetadataVm>
            {
                new SensorMetadataVm
                {
                    Key = "Temperature",
                    Value = "75°F"
                },
                new SensorMetadataVm
                {
                    Key = "Humidity",
                    Value = "50%"
                }
            }
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new SingleMetadataConverter() }
        };

        var expectedJson = JsonSerializer.Serialize(new
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789abc"),
            Name = "Example Sensor",
            Metadata = new
            {
                Temperature = "75°F",
                Humidity = "50%"
            }
        }, options);

        // Act
        string actualJson = JsonSerializer.Serialize(sensor, options);

        // Assert
        Assert.Equal(expectedJson.Trim(), actualJson);
    }

    // Test to assert the read operation of the SingleMetadataConverter
    [Fact]
    public void Deserialize_SensorVm_CorrectObjectOutput()
    {
        // Arrange
        var json = @"{
            ""id"": ""12345678-1234-1234-1234-123456789abc"",
            ""name"": ""Example Sensor"",
            ""metadata"": {
                ""Temperature"": ""75°F"",
                ""Humidity"": ""50%""
            }
        }";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new SingleMetadataConverter() }
        };

        var expectedSensor = new SensorVm
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789abc"),
            Name = "Example Sensor",
            Metadata = new List<SensorMetadataVm>
            {
                new SensorMetadataVm
                {
                    Key = "Temperature",
                    Value = "75°F"
                },
                new SensorMetadataVm
                {
                    Key = "Humidity",
                    Value = "50%"
                }
            }
        };

        // Act
        var actualSensor = JsonSerializer.Deserialize<SensorVm>(json, options);

        // Assert
        Assert.Equal(expectedSensor.Id, actualSensor?.Id);
        Assert.Equal(expectedSensor.Name, actualSensor?.Name);
        Assert.Equal(expectedSensor.Metadata.Count, actualSensor?.Metadata.Count);

        for (int i = 0; i < expectedSensor.Metadata.Count; i++)
        {
            Assert.Equal(expectedSensor.Metadata[i].Key, actualSensor?.Metadata[i].Key);
            Assert.Equal(expectedSensor.Metadata[i].Value, actualSensor?.Metadata[i].Value);
        }
    }

    /*
        Test deserialization of array metadata

        @"{
            ""id"": ""12345678-1234-1234-1234-123456789abc"",
            ""name"": ""Example Sensor"",
            ""metadata"": [
                {
                    ""key"": ""Temperature"",
                    ""value"": ""75°F""
                },
                {
                    ""key"": ""Humidity"",
                    ""value"": ""50%""
                }
            ]
        }";
     */

    [Fact]
    public void Deserialize_SensorVmArrayMetadata_CorrectObjectOutput()
    {
        // Arrange
        var json = @"{
            ""id"": ""12345678-1234-1234-1234-123456789abc"",
            ""name"": ""Example Sensor"",
            ""metadata"": [
                {
                    ""key"": ""Temperature"",
                    ""value"": ""75°F""
                },
                {
                    ""key"": ""Humidity"",
                    ""value"": ""50%""
                }
            ]
        }";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new SingleMetadataConverter() }
        };

        var expectedSensor = new SensorVm
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789abc"),
            Name = "Example Sensor",
            Metadata = new List<SensorMetadataVm>
            {
                new SensorMetadataVm
                {
                    Key = "Temperature",
                    Value = "75°F"
                },
                new SensorMetadataVm
                {
                    Key = "Humidity",
                    Value = "50%"
                }
            }
        };

        // Act
        var actualSensor = JsonSerializer.Deserialize<SensorVm>(json, options);

        // Assert
        Assert.Equal(expectedSensor.Id, actualSensor?.Id);
        Assert.Equal(expectedSensor.Name, actualSensor?.Name);
        Assert.Equal(expectedSensor.Metadata.Count, actualSensor?.Metadata.Count);

        for (int i = 0; i < expectedSensor.Metadata.Count; i++)
        {
            Assert.Equal(expectedSensor.Metadata[i].Key, actualSensor?.Metadata[i].Key);
            Assert.Equal(expectedSensor.Metadata[i].Value, actualSensor?.Metadata[i].Value);
        }
    }
}