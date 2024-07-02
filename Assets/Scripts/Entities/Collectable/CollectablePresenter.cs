using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour
{
    public int UniqueId { get; set; }
    
    [SerializeField] private SpriteRenderer _renderer;

    public CollectableModel Model => _model;
    
    [Inject] private CollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;
    
    private void Awake()
    {
        _model.Sprite.Where(s => s != null).Subscribe(HandleSprite).AddTo(this);
    }

    private void HandleSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}

public class CollectableFactory : PlaceholderFactory<CollectablePresenter> { }