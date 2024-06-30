﻿using System;
using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour, IPoolable<CollectableType, IMemoryPool>, IDisposable
{
    [SerializeField] private SpriteRenderer _renderer;

    public CollectableModel Model => _model;
    
    [Inject] private CollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;

    private IMemoryPool _pool;
    
    private void Awake()
    {
        _model.Sprite.Subscribe(HandleSprite);
        _model.IsVisible.Subscribe(HandleVisibility);
        _model.Position.Subscribe(HandlePosition);
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

    public void OnSpawned(CollectableType type, IMemoryPool pool)
    {
        _pool = pool;
        _model.Initialize(type);
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

public class CollectableFactory : PlaceholderFactory<CollectableType, CollectablePresenter> { }