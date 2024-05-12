using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tributech.SensorManager.Domain.Helpers;

internal class FuncEqualityComparer<T>(Func<T, T, bool> comparer, Func<T, int> hash) : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _comparer = comparer;
    private readonly Func<T, int> _hash = hash;

    public bool Equals(T x, T y) => _comparer(x, y);

    public int GetHashCode(T obj) => _hash(obj);

    public static FuncEqualityComparer<T> Create(Func<T, T, bool> comparer, Func<T, int> hash) => new(comparer, hash);
}

internal static class FuncEqualityComparer
{
    public static FuncEqualityComparer<T> Create<T>(Func<T, T, bool> comparer, Func<T, int> hash) => FuncEqualityComparer<T>.Create(comparer, hash);
}