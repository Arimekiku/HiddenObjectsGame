using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CounterProvider : MonoBehaviour
{
    [SerializeField] private Canvas _parent;
    
    [Inject] private CollectableUIFactory _uiFactory;
    [Inject] private CollectableCounterFactory _counterFactory;
    [Inject] private SaveProvider _saveProvider;
    [Inject] private CameraTracker _cameraTracker;
    [Inject] private SpriteProvider _spriteProvider;
    [Inject] private CurrencyProvider _currencyProvider;
    
    private readonly List<CounterPresenter> _holders = new();

    public void CreateCounters(List<CollectablePresenter> collectables, List<ProducerPresenter> producers)
    {
        _holders.Add(_currencyProvider.MoneyCounterPresenter);
        _holders.Add(_currencyProvider.StarCounterPresenter);
        
        foreach (var collectable in collectables)
        {
            int spriteCode = collectable.Model.Sprite.Value.GetHashCode();
            
            CounterPresenter counterPresenter = CreateUiCounter(spriteCode);
            counterPresenter?.UpdateImage(collectable.Model.Sprite.Value);
        }
        
        foreach (var producer in producers)
        {
            int spriteCode = producer.CreateId;

            CounterPresenter counterPresenter = CreateUiCounter(spriteCode);
            counterPresenter?.UpdateImage(_spriteProvider.GetConcreteSprite(spriteCode));
        }

        foreach (var counter in _holders)
        {
            foreach (var counterData in _saveProvider.SaveData.CountersData)
            {
                if (counterData.Id != counter.CounterModel.Id.Value) 
                    continue;
                
                counter.CounterModel.Count.Value = counterData.Count;
            }
        }
    }

    public void ClearCounters()
    {
        _holders.Remove(_currencyProvider.MoneyCounterPresenter);
        _holders.Remove(_currencyProvider.StarCounterPresenter);
        
        foreach (var counter in _holders)
            Destroy(counter.gameObject);
        _holders.Clear();
    }

    public void CollectAnimation(CollectablePresenter presenter)
    {
        CollectableUIPresenter instance = _uiFactory.Create(presenter);
        instance.transform.SetParent(_parent.transform, false);
        instance.transform.position = _cameraTracker.MainCamera.WorldToScreenPoint(presenter.transform.position);

        CounterPresenter counterPresenterUI = _holders.First(h => h.CounterModel.Id.Value == presenter.Model.Sprite.Value.GetHashCode());
        
        counterPresenterUI.CounterModel.AddDelta();

        int counterId = counterPresenterUI.CounterModel.Id.Value;
        _saveProvider.SaveData.UpdateCounter(counterId, counterPresenterUI.CounterModel.Count.Value);
        _saveProvider.CurrencyData.TrySave(counterId, counterPresenterUI.CounterModel.Count.Value);
        _saveProvider.Save();
        
        instance.transform
            .DOMove(counterPresenterUI.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                counterPresenterUI.UpdateText();
                Destroy(presenter.gameObject);
                Destroy(instance.gameObject);
            });
    }

    private CounterPresenter CreateUiCounter(int spriteCode)
    {
        if (_holders.Any(h => h.CounterModel.Id.Value == spriteCode))
            return null;
            
        CounterPresenter uiCounterPresenter = _counterFactory.Create();
        uiCounterPresenter.Initialize(spriteCode);
        _saveProvider.SaveData.AddCounter(uiCounterPresenter.CounterModel.Id.Value);
        
        uiCounterPresenter.transform.SetParent(transform, false);
        uiCounterPresenter.UpdateText();
            
        _holders.Add(uiCounterPresenter);
        return uiCounterPresenter;
    }
}