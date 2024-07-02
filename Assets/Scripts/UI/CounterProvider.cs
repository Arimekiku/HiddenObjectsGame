using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ModestTree;
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
        LoadCurrency();
        if (TryLoadCountersFromFile())
            return;
        
        LoadFromCollection(collectables, producers);
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
        instance.transform.rotation = presenter.transform.rotation;
        instance.transform.localScale = presenter.transform.localScale;

        CounterPresenter counterPresenterUI = _holders.First(h => h.CounterModel.Id.Value == presenter.Model.Sprite.Value.GetHashCode());
        Destroy(presenter.gameObject);

        counterPresenterUI.CounterModel.AddDelta();

        int counterId = counterPresenterUI.CounterModel.Id.Value;
        _saveProvider.SaveData.UpdateCounter(counterId, counterPresenterUI.CounterModel.Count.Value);
        _saveProvider.CurrencyData.TrySave(counterId, counterPresenterUI.CounterModel.Count.Value);
        _saveProvider.Save();

        instance.transform.DORotate(counterPresenterUI.transform.rotation.eulerAngles, 0.5f);
        instance.transform.DOScale(counterPresenterUI.transform.localScale, 0.5f);
        instance.transform
            .DOMove(counterPresenterUI.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                counterPresenterUI.UpdateText();
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

    private bool TryLoadCountersFromFile()
    {
        if (_saveProvider.SaveData.CountersData.IsEmpty())
            return false;
        
        foreach (var counterData in _saveProvider.SaveData.CountersData)
        {
            int spriteCode = counterData.Id;
            if (_holders.Any(c => c.CounterModel.Id.Value == spriteCode)) 
                continue;
            
            CounterPresenter counterPresenter = CreateUiCounter(spriteCode);
                
            Sprite counterSprite = _spriteProvider.GetConcreteSprite(spriteCode);
            if (counterPresenter == null) 
                continue;
            
            counterPresenter.UpdateImage(counterSprite);
            counterPresenter.CounterModel.Count.Value = counterData.Count;
            counterPresenter.UpdateText();
        }

        return true;
    }

    private void LoadFromCollection(List<CollectablePresenter> collectables, List<ProducerPresenter> producers)
    {
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
    }

    private void LoadCurrency()
    {
        _holders.Add(_currencyProvider.MoneyCounterPresenter);
        _holders.Add(_currencyProvider.StarCounterPresenter);
        
        foreach (var currencyData in _saveProvider.CurrencyData.CountersData)
        {
            var holder = _holders.First(c => c.CounterModel.Id.Value == currencyData.Id);
            holder.CounterModel.Count.Value = currencyData.Count;
            holder.UpdateText();
        }
    }
}