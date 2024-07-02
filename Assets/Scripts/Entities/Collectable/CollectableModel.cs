using UniRx;
using UnityEngine;

public class CollectableModel
{
    public IReadOnlyReactiveProperty<Sprite> Sprite => _sprite;
    public ReactiveCommand OnCollect { get; }

    private readonly ReactiveProperty<Sprite> _sprite;

    public CollectableModel()
    {
        _sprite = new ReactiveProperty<Sprite>(null);

        OnCollect = new ReactiveCommand();
    }

    public void Collect()
    {
        OnCollect.Execute();
    }

    public void UpdateSprite(Sprite sprite)
    {
        _sprite.Value = sprite;
    }
}