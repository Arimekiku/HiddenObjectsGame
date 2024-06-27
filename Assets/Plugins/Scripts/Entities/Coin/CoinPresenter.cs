﻿using UniRx;
using UnityEngine;
using Zenject;

public class CoinPresenter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    [Inject] private CoinData _data;
    [Inject] private CollectableModel _model;

    private void Awake()
    {
        _renderer.sprite = _data.Sprite;
        
        _model.IsCollected.Subscribe(HandleDeath);
    }

    private void HandleDeath(bool isDead)
    {
        if (isDead == false)
            return;
        
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        _model.Collect();
    }
}

public class CoinFactory : PlaceholderFactory<CoinData, CoinPresenter> { }