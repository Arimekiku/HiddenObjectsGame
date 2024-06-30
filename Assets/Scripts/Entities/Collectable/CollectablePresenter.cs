using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    public CollectableModel Model => _model;
    
    [Inject] private CollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;
    
    private void Awake()
    {
        _model.Sprite.Subscribe(HandleSprite).AddTo(this);
    }

    private void HandleSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void Initialize(CollectableType type)
    {
        _model.Initialize(type);
    }
}

public class CollectableFactory : PlaceholderFactory<CollectablePresenter> { }