using UniRx;

public interface ICollectableModel
{
    public BoolReactiveProperty IsCollected { get; }
    
    public void Collect();
}