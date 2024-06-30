using UniRx;

public interface ICounter
{
    public CollectableType Type { get; }
    public IntReactiveProperty Count { get; }

    public void Initialize(CollectableType type);
    
    public void Add(int value);
    public bool TryRemove(int value);
}