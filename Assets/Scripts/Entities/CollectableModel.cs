using UniRx;
using UnityEngine;

public class CollectableModel
{
    public CollectableType Type { get; private set; }
    
    public BoolReactiveProperty IsVisible { get; }
    public ReactiveProperty<Sprite> Sprite { get; }
    public Vector3ReactiveProperty Position { get; }
    
    public ReactiveCommand OnCollect { get; }

    public CollectableModel()
    {
        IsVisible = new BoolReactiveProperty();
        Sprite = new ReactiveProperty<Sprite>();
        Position = new Vector3ReactiveProperty();

        OnCollect = new ReactiveCommand();
        
        Type = CollectableType.Empty;
    }

    public void Initialize(CollectableType type)
    {
        IsVisible.Value = true;
        Type = type;
    }

    public void Dispose()
    {
        IsVisible.Value = false;
        Sprite.Value = null;
        Position.Value = Vector3.zero;
        Type = CollectableType.Empty;
    }

    public void Collect()
    {
        OnCollect.Execute();
    }

    public void UpdateSprite(Sprite sprite)
    {
        Sprite.Value = sprite;
    }
}