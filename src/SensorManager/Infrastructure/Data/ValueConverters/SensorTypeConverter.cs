using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Domain.ValueObjects;

namespace Tributech.SensorManager.Infrastructure.Data.ValueConverters;

// ValueConverter for EfCore
public class SensorTypeConverter : ValueConverter<SensorType, string>
{
    public SensorTypeConverter(ConverterMappingHints mappingHints = null) : base
    (
        v => v.Value,
        v => SensorType.Parse(v),
        mappingHints
    )
    {
    }
}