using UniRx;
using UnityEngine;

public class CollectableModel
{
    public IReadOnlyReactiveProperty<Sprite> Sprite => _sprite;
    public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;
    public ReactiveCommand OnCollect { get; }

    private readonly ReactiveProperty<Sprite> _sprite;
    private readonly BoolReactiveProperty _isEnabled;

    public CollectableModel()
    {
        _sprite = new ReactiveProperty<Sprite>(null);
        _isEnabled = new BoolReactiveProperty();

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

    public void UpdateVisibility(bool isVisible)
    {
        _isEnabled.Value = isVisible;
    }
}