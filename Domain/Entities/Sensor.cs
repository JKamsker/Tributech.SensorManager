using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tributech.SensorManager.Domain.Entities;
public class Sensor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<SensorMetadata> Metadata { get; set; } = new List<SensorMetadata>();
}

public class SensorMetadata
{
    public string Key { get; set; }
    public string Value { get; set; }
}