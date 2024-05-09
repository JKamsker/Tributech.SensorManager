using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Application.Sensors.Queries.Common;

public class SensorVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SensorMetadataVm> Metadata { get; set; } = new();

    public SensorVm()
    {
    }

    public SensorVm(Guid id, string name, List<SensorMetadataVm> metadata)
    {
        Id = id;
        Name = name;
        Metadata = metadata;
    }

    public SensorVm(Sensor sensor)
    {
        Id = sensor.Id;
        Name = sensor.Name;
        Metadata = sensor.Metadata.Select(m => new SensorMetadataVm(m.Key, m.Value)).ToList();
    }
}