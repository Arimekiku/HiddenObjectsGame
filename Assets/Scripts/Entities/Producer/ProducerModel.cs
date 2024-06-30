using UniRx;
using UnityEngine;

public class ProducerModel
{
    public ReactiveCommand OnCollect { get; }
    
    public BoolReactiveProperty IsCollected { get; }
    public ReactiveProperty<Sprite> Sprite { get; }

    public ProducerModel()
    {
        OnCollect = new ReactiveCommand();
        IsCollected = new BoolReactiveProperty();
        Sprite = new ReactiveProperty<Sprite>();
    }

    public void Initialize(bool collected)
    {
        IsCollected.Value = collected;
    }

    public void Collect()
    {
        if (IsCollected.Value == true) 
            return;

        OnCollect.Execute();
        IsCollected.Value = true;
    }

    public void UpdateSprite(Sprite sprite)
    {
        Sprite.Value = sprite;
    }
}