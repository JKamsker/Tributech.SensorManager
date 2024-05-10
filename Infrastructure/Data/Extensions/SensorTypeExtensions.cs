using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.Linq.Expressions;

using Tributech.SensorManager.Domain.ValueTypes;
using Tributech.SensorManager.Infrastructure.Data.ValueComparer;
using Tributech.SensorManager.Infrastructure.Data.ValueConverters;

namespace Tributech.SensorManager.Infrastructure.Data.Extensions;

internal static class SensorTypeExtensions
{
    public static PropertyBuilder<SensorType> SensorType<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, SensorType>> propertyExpression)
        where T : class
    {
        return builder
            .Property(propertyExpression)
            .IsSensorType();
    }

    public static PropertyBuilder<SensorType> IsSensorType(this PropertyBuilder<SensorType> propertyBuilder)
    {
        return propertyBuilder.HasConversion<string>(new SensorTypeConverter(), new SensorTypeComparer());
    }
}