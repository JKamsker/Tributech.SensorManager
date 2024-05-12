namespace Tributech.SensorManager.Domain.Abstractions;

public interface IKeyValuePair<TKey, TValue>
{
    TKey Key { get; set; }
    TValue Value { get; set; }
}