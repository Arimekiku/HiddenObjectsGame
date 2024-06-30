using UniRx;
using UnityEngine;
using Zenject;

public class ProducerPresenter : MonoBehaviour
{
    public Bounds SpawnBounds => _spawnBounds.bounds;
    
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BoxCollider2D _spawnBounds;

    [Inject] private ProducerModel _producerModel;

    public ProducerModel Model => _producerModel;

    private void Awake()
    {
        _producerModel.Sprite.Subscribe(HandleSprite);
        _producerModel.IsCollected.Subscribe(HandleOpacity);
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