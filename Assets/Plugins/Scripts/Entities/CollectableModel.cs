using UniRx;

public class CollectableModel : ICollectableModel
{
    public BoolReactiveProperty IsCollected { get; }

    public CollectableModel()
    {
        IsCollected = new BoolReactiveProperty(false);
    }

    public void Collect()
    {
        IsCollected.Value = true;
    }
}