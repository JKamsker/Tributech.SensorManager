using Microsoft.EntityFrameworkCore.ChangeTracking;

using Tributech.SensorManager.Domain.ValueTypes;

namespace Tributech.SensorManager.Infrastructure.Data.ValueComparer;

// ValueComparer for EfCore

public class SensorTypeComparer : ValueComparer<SensorType>
{
    public SensorTypeComparer() : base
    (
        equalsExpression: (c1, c2) => c1.Value == c2.Value,
        hashCodeExpression: c => c.Value.GetHashCode(),
        snapshotExpression: c => c
    )
    {
    }
}