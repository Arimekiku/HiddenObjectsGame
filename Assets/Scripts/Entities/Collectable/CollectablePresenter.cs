using System;
using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour, IDisposable
{
    [SerializeField] private SpriteRenderer _renderer;

    public CollectableModel Model => _model;
    
    [Inject] private CollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;

    private IMemoryPool _pool;
    
    private void Awake()
    {
        _model.Sprite.Subscribe(HandleSprite);
    }

    private void HandleSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void Initialize(CollectableType type)
    {
        _model.Initialize(type);
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }
}

public class CollectableFactory : PlaceholderFactory<CollectablePresenter> { }