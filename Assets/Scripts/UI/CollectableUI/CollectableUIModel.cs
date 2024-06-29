using UniRx;
using UnityEngine;

public class CollectableUIModel
{
    public ReactiveProperty<CollectableType> Type { get; }
    public ReactiveProperty<Sprite> Sprite { get; }

    public CollectableUIModel()
    {
        Type = new ReactiveProperty<CollectableType>();
        Sprite = new ReactiveProperty<Sprite>();
    }

    public void Initialize(CollectableType type, Sprite sprite)
    {
        Type.Value = type;
        Sprite.Value = sprite;
    }
}