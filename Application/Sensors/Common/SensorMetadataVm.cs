using System.Diagnostics;

namespace Tributech.SensorManager.Application.Sensors.Common;

[DebuggerDisplay("{Key,nq}: {Value,nq}")]
public class SensorMetadataVm
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public SensorMetadataVm()
    {
    }

    public SensorMetadataVm(string key, string value)
    {
        Key = key;
        Value = value;
    }
}