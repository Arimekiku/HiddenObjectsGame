using UniRx;

public interface ICollectableModel
{
    public CollectableData Data { get; }
    
    public BoolReactiveProperty IsCollected { get; }
    
    public void Collect();
}