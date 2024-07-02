using UniRx;

public interface ICounter
{
    public IntReactiveProperty Count { get; }

    public void Add(int value);
    public bool TryRemove(int value);
}