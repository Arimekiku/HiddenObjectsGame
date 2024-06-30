using UniRx;
using UnityEngine;

public class CollectableModel
{
    public CollectableType Type { get; private set; }
    
    public ReactiveProperty<Sprite> Sprite { get; }
    
    public ReactiveCommand OnCollect { get; }

    public CollectableModel()
    {
        Sprite = new ReactiveProperty<Sprite>();

        OnCollect = new ReactiveCommand();
        
        Type = CollectableType.Empty;
    }

    public void Initialize(CollectableType type)
    {
        Type = type;
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