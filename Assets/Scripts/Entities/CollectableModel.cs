using UniRx;

public class CollectableModel : ICollectableModel
{
    public CollectableType Type { get; }
    public BoolReactiveProperty IsCollected { get; }

    public CollectableModel(CollectableType type)
    {
        IsCollected = new BoolReactiveProperty(false);

        Type = type;
    }

    public void Collect()
    {
        IsCollected.Value = true;
    }
}