using System;
using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour, IPoolable<Sprite, CollectableType, IMemoryPool>, IDisposable
{
    [SerializeField] private SpriteRenderer _renderer;

    public CollectableModel Model => _model;
    
    [Inject] private CollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;

    private IMemoryPool _pool;
    
    private void Awake()
    {
        _model.IsCollected.Subscribe(HandleDeath);
        _model.Sprite.Subscribe(HandleSprite);
        _model.IsVisible.Subscribe(HandleVisibility);
        _model.Position.Subscribe(HandlePosition);
    }
    
    public void Collect()
    {
        _model.Collect();
    }

    private void HandleDeath(bool value)
    {
        if (value == false)
            return;
        
        DestroyImmediate(gameObject);
    }

    private void HandleSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    private void HandlePosition(Vector3 position)
    {
        transform.position = position;
    }

    private void HandleVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void OnSpawned(Sprite sprite, CollectableType type, IMemoryPool pool)
    {
        _pool = pool;
        _model.Initialize(sprite, type);
    }

    public void OnDespawned()
    {
        _pool = null;
        _model.Dispose();
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }
}

public class CollectableFactory : PlaceholderFactory<Sprite, CollectableType, CollectablePresenter> { }