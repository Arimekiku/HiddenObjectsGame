using UniRx;
using UnityEngine;

public class CollectableModel
{
    public CollectableType Type { get; private set; }
    
    public BoolReactiveProperty IsCollected { get; }
    public BoolReactiveProperty IsVisible { get; }
    public ReactiveProperty<Sprite> Sprite { get; }
    public Vector3ReactiveProperty Position { get; }

    public CollectableModel()
    {
        IsCollected = new BoolReactiveProperty(false);
        IsVisible = new BoolReactiveProperty(true);
        Sprite = new ReactiveProperty<Sprite>();
        Position = new Vector3ReactiveProperty();
        Type = CollectableType.Empty;
    }

    public void Initialize(Sprite sprite, CollectableType type)
    {
        IsVisible.Value = false;
        Sprite.Value = sprite;
        Type = type;
    }

    public void Dispose()
    {
        IsCollected.Value = false;
        IsVisible.Value = false;
        Sprite.Value = null;
        Position.Value = Vector3.zero;
        Type = CollectableType.Empty;
    }

    public void Collect()
    {
        IsCollected.Value = true;
    }

    public void UpdateSprite(Sprite sprite)
    {
        Sprite.Value = sprite;
    }
}