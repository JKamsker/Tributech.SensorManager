using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Tributech.SensorManager.Infrastructure.Data.ValueConverters;

// DataType
public class DataTypeConverter : ValueConverter<Domain.ValueObjects.FieldType, string>
{
    public DataTypeConverter(ConverterMappingHints mappingHints = null) : base
    (
        v => v.Value,
        v => Domain.ValueObjects.FieldType.Parse(v),
        mappingHints
    )
    {
    }
}