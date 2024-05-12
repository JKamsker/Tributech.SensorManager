using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tributech.SensorManager.Infrastructure.Data.ValueComparer;

// DataType
public class DataTypeComparer : ValueComparer<Domain.ValueObjects.ValueType>
{
    public DataTypeComparer() : base
    (
        equalsExpression: (c1, c2) => c1.Value == c2.Value,
        hashCodeExpression: c => c.Value.GetHashCode(),
        snapshotExpression: c => c
    )
    {
    }
}