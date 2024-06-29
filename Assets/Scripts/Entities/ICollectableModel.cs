using UniRx;

public interface ICollectableModel
{
    public CollectableType Type { get; }
    
    public BoolReactiveProperty IsCollected { get; }
    
    public void Collect();
}