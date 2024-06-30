using UniRx;

public class Counter : ICounter
{
    public CollectableType Type { get; private set; }
    public IntReactiveProperty Count { get; }
    
    public Counter()
    {
        Count = new IntReactiveProperty(0);
        
        Type = CollectableType.Empty;
    }

    public void Initialize(CollectableType type)
    {
        Type = type;
    }
    
    public void Add(int value)
    {
        Count.Value += value;
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