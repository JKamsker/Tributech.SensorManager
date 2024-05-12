using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Tributech.SensorManager.Infrastructure.Data.ValueConverters;

// DataType
public class DataTypeConverter : ValueConverter<Domain.ValueObjects.ValueType, string>
{
    public DataTypeConverter(ConverterMappingHints mappingHints = null) : base
    (
        v => v.Value,
        v => Domain.ValueObjects.ValueType.Parse(v),
        mappingHints
    )
    {
    }
}