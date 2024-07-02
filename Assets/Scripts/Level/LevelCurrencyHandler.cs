using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class LevelCurrencyHandler : DisposableEntity
{
    public ReactiveCommand<CollectablePresenter> OnCurrencyCollected { get; private set; }
    
    [Inject] private SpriteProvider _spriteProvider;
    [Inject] private ILevelSpawner _levelSpawner;
    
    private readonly List<CollectablePresenter> _currency;
    private readonly List<CollectablePresenter> _stars;

    public LevelCurrencyHandler()
    {
        OnCurrencyCollected = new ReactiveCommand<CollectablePresenter>();
        _currency = new List<CollectablePresenter>();
    }

    public void SpawnCoin(LevelData data, Transform center)
    {
        Sprite coinSprite = _spriteProvider.GetCoinSprite();
        var coin = _levelSpawner.SpawnAndPlaceCollectable(data.MaxSpawnRadius, center, coinSprite);
        coin.Model.OnCollect.Subscribe(_ => OnCurrencyCollect(data, center, coin));
        coin.Model.UpdateVisibility(true);
        
        _currency.Add(coin);
    }

    public void SpawnStar(LevelData data, Transform center)
    {
        Sprite starSprite = _spriteProvider.GetStarSprite();
        var star = _levelSpawner.SpawnAndPlaceCollectable(data.MaxSpawnRadius, center, starSprite);
        star.Model.OnCollect.Subscribe(_ => OnCurrencyCollect(data, center, star));
        star.Model.UpdateVisibility(true);
        
        _currency.Add(star);
    }

    public void Clear()
    {
        Dispose();
        OnCurrencyCollected = new ReactiveCommand<CollectablePresenter>();

        foreach (var coin in _currency)
            Object.Destroy(coin.gameObject);
        _currency.Clear();
    }
    
    private void OnCurrencyCollect(LevelData data, Transform center, CollectablePresenter currency)
    {
        _currency.Remove(currency);
        
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ =>
        {
            var newCurrency = _levelSpawner.SpawnAndPlaceCollectable(data.MaxSpawnRadius, center, currency.Model.Sprite.Value);
            newCurrency.Model.OnCollect.Subscribe(_ => OnCurrencyCollect(data, center, newCurrency));
            newCurrency.Model.UpdateVisibility(true);
        }).AddTo(this);
        
        OnCurrencyCollected.Execute(currency);
    }
}