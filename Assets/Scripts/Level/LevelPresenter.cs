using System.Collections;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelPresenter : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _mapBounds;
    
    [Inject] private LevelCurrencyData _currencyData;
    [Inject] private LevelModel _model;
    [Inject] private CounterProvider _uiSpawner;
    [Inject] private ILevelSwapper _levelSwapper;
    
    private void Awake()
    {
        _model.SpawnEntitiesInBounds(_mapBounds.bounds);

        foreach (CollectablePresenter coin in _model.Coins)
            coin.Model.OnCollect.Subscribe(_ => OnCoinClick(coin));
        
        foreach (CollectablePresenter star in _model.Stars)
            star.Model.OnCollect.Subscribe(_ => OnStarClick(star));
        
        foreach (CollectablePresenter hiddenObject in _model.HiddenObjects)
            hiddenObject.Model.OnCollect.Subscribe(_ => OnHiddenObjectClick(hiddenObject));
    }

    private void OnCoinClick(CollectablePresenter coin)
    {
        coin.gameObject.SetActive(false);
        
        _uiSpawner.CollectAnimation(coin, _currencyData.CoinsAmount);

        Observable.FromCoroutine(_ => Timer(10f)).Subscribe(_ =>
        {
            CollectablePresenter newCoin = _model.Coins.First(c => c.Model.IsVisible.Value == false);
            newCoin.gameObject.SetActive(true);
        });
    }

    private void OnStarClick(CollectablePresenter star)
    {
        star.gameObject.SetActive(false);
        
        _uiSpawner.CollectAnimation(star, _currencyData.StarAmount);
        
        Timer(10f).ToObservable().Subscribe(_ =>
        {
            CollectablePresenter newStar = _model.Stars.First(c => c.Model.IsVisible.Value == false);
            newStar.gameObject.SetActive(true);
        });
    }

    private void OnHiddenObjectClick(CollectablePresenter hiddenObject)
    {
        if (_model.HiddenObjects.IsEmpty())
        {
            _levelSwapper.LoadNextLevel();
            return;
        }
        
        hiddenObject.gameObject.SetActive(false);
        
        _uiSpawner.CollectAnimation(hiddenObject, 1);
        
        Timer(10f).ToObservable().Subscribe(_ =>
        {
            CollectablePresenter newStar = _model.HiddenObjects.First(c => c.Model.IsVisible.Value == false);
            newStar.gameObject.SetActive(true);
        });
    }

    private IEnumerator Timer(float time)
    {
        float timer = time;

        while (timer >= 0)
        {
            yield return new WaitForFixedUpdate();
            
            timer -= Time.fixedDeltaTime;
        }
    }
}