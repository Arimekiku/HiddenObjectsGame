using UniRx;

public class CollectableModel : ICollectableModel
{
    public CollectableData Data { get; }
    
    public BoolReactiveProperty IsCollected { get; }

    public CollectableModel(CollectableData data)
    {
        Data = data;
        
        IsCollected = new BoolReactiveProperty(false);
    }

    public void Collect()
    {
        IsCollected.Value = true;
    }
}