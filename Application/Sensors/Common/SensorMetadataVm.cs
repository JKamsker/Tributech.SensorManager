using System.Collections.Generic;
using System.Diagnostics;

using Tributech.SensorManager.Domain.Abstractions;

namespace Tributech.SensorManager.Application.Sensors.Common;

[DebuggerDisplay("{Key,nq}: {Value,nq}")]
public class SensorMetadataVm : IKeyValuePair<string, string>
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

    // implicit to KeyValuePair<string, string>
    public static implicit operator KeyValuePair<string, string>(SensorMetadataVm metadata)
    {
        return new KeyValuePair<string, string>(metadata.Key, metadata.Value);
    }
}