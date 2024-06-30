using UniRx;
using UnityEngine;
using Zenject;

public class ProducerPresenter : MonoBehaviour
{
    public int Id;
    
    [SerializeField] private SpriteRenderer _renderer;

    [Inject] private ProducerModel _producerModel;

    public ProducerModel Model => _producerModel;

    private void Awake()
    {
        _producerModel.Sprite.Subscribe(HandleSprite).AddTo(this);
        _producerModel.IsCollected.Subscribe(HandleOpacity).AddTo(this);
    }

    private void HandleSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    private void HandleOpacity(bool collected)
    {
        if (collected == false)
            return;
        
        _renderer.color = new Color(
            _renderer.color.r, 
            _renderer.color.g, 
            _renderer.color.b, 
            _renderer.color.a / 10f);
    }
}

public class ProducerFactory : PlaceholderFactory<ProducerPresenter> { }