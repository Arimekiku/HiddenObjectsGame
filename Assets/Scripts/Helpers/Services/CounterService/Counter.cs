using UniRx;

public class Counter : ICounter
{
    public IntReactiveProperty Count { get; }
    
    public Counter()
    {
        Count = new IntReactiveProperty(0);
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