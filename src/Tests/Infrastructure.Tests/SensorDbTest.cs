using Microsoft.EntityFrameworkCore;

using System;

using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Infrastructure.Data;

namespace Infrastructure.Tests;

public class SensorDbTest
{
    private SensorDbContext _dbContext;

    public SensorDbTest()
    {
        var options = new DbContextOptionsBuilder<SensorDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb")
           .Options;

        _dbContext = new SensorDbContext(options);

        _dbContext.Sensors.Add(new Sensor
        {
            Name = "TestSensor",
            //Type = "Temperature",
            Metadata = new List<SensorMetadata>
            {
                new SensorMetadata { Key = "Location", Value = "Living Room" },
                new SensorMetadata { Key = "Manufacturer", Value = "Test Inc." }
            }
        });

        _dbContext.SaveChanges();
    }

    [Fact]
    public void Test_SensorDbContext()
    {
        var sensor = _dbContext.Sensors.FirstOrDefault(s => s.Name == "TestSensor");

        Assert.NotNull(sensor);
        //Assert.Equal("Temperature", sensor.Type);
        Assert.Equal(2, sensor.Metadata.Count);

        var location = sensor.Metadata.FirstOrDefault(m => m.Key == "Location");
        Assert.NotNull(location);
        Assert.Equal("Living Room", location.Value);
    }
}