using UniRx;
using UnityEngine;

public class CounterModel : ICounterModel
{
    public IntReactiveProperty Id { get; }
    public IntReactiveProperty Count { get; }
    public Vector3ReactiveProperty Position { get; }
    public int Delta { get; private set; }

    public CounterModel()
    {
        Id = new IntReactiveProperty();
        Count = new IntReactiveProperty(0);
        Position = new Vector3ReactiveProperty();
    }

    public void Initialize(Vector3 position, int id, int delta = 1)
    {
        Position.Value = position;
        
        Id.Value = id;
        
        Delta = delta;
    }
    
    public void AddDelta()
    {
        Count.Value += Delta;
    }

    public bool TryRemove(int value)
    {
        if (Count.Value < value)
            return false;

        Count.Value -= value;
        return true;
    }

    public void Load(int value)
    {
        Count.Value = value;
    }
}