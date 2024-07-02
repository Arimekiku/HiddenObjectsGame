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
    
    private List<CollectableUICounter> _holders;
    
    public void CreateCounters(List<CollectablePresenter> collectables, List<ProducerPresenter> producers)
    {
        _holders = new List<CollectableUICounter>();
        
        foreach (var collectable in collectables)
        {
            int spriteCode = collectable.Model.Sprite.Value.GetHashCode();
            
            CollectableUICounter counter = CreateUiCounter(spriteCode);
            counter?.UpdateImage(collectable.Model.Sprite.Value);
        }
        
        foreach (var producer in producers)
        {
            int spriteCode = producer.CreateId;

            CollectableUICounter counter = CreateUiCounter(spriteCode);
            counter?.UpdateImage(_spriteProvider.GetConcreteSprite(spriteCode));
        }

        foreach (var counter in _holders)
        {
            foreach (var counterData in _saveProvider.SaveData.CountersData)
            {
                if (counterData.Id != counter.Id) 
                    continue;
                
                counter.Counter.Count.Value = counterData.Count;
            }
        }
    }

    public void CollectAnimation(CollectablePresenter presenter)
    {
        CollectableUIPresenter instance = _uiFactory.Create(presenter);
        instance.transform.SetParent(_parent.transform, false);
        instance.transform.position = _cameraTracker.MainCamera.WorldToScreenPoint(presenter.transform.position);

        CollectableUICounter counterUI = _holders.First(h => h.Id == presenter.Model.Sprite.Value.GetHashCode());
        
        //TODO: make this frexible
        counterUI.Counter.Add(1);
        
        _saveProvider.SaveData.UpdateCounter(counterUI.Id, counterUI.Counter.Count.Value);
        _saveProvider.CurrencyData.TrySave(counterUI.Id, counterUI.Counter.Count.Value);
        _saveProvider.Save();
        
        instance.transform
            .DOMove(counterUI.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                counterUI.UpdateText();
                DestroyImmediate(presenter.gameObject);
                DestroyImmediate(instance.gameObject);
            });
    }

    private CollectableUICounter CreateUiCounter(int spriteCode)
    {
        if (_holders.Any(h => h.Id == spriteCode))
            return null;
            
        CollectableUICounter uiCounter = _counterFactory.Create(spriteCode);
        _saveProvider.SaveData.AddCounter(uiCounter.Id);
        
        uiCounter.transform.SetParent(transform, false);
        uiCounter.UpdateText();
            
        _holders.Add(uiCounter);
        return uiCounter;
    }
}