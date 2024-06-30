using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CounterProvider : MonoBehaviour
{
    [SerializeField] private Canvas _parent;
    [SerializeField] private CollectableUICounter[] _holders;

    [Inject] private CollectableUIFactory _uiFactory;
    [Inject] private CameraTracker _tracker;
    [Inject] private SaveProvider _saveProvider;
    [Inject] private CameraTracker _cameraTracker;

    private void Awake()
    {
        List<CounterSaveData> allCounters = new List<CounterSaveData>(_saveProvider.CurrencyData.CountersData);
        allCounters.AddRange(_saveProvider.SaveData.CountersData);
        
        foreach (CounterSaveData counterData in allCounters)
        {
            foreach (CollectableUICounter uiCounter in _holders)
            {
                if (uiCounter.Counter.Type != counterData.Type) 
                    continue;
                
                uiCounter.Counter.Count.Value = counterData.Count;
                uiCounter.UpdateText();
                break;
            }
        }
    }

    public void CollectAnimation(CollectablePresenter presenter, int changeValue)
    {
        CollectableUIPresenter instance = _uiFactory.Create(presenter.Model);
        instance.transform.SetParent(_parent.transform, false);
        instance.transform.position = _cameraTracker.MainCamera.WorldToScreenPoint(presenter.transform.position);

        CollectableUICounter counterUI = _holders.First(h => h.Counter.Type == presenter.Model.Type);
        
        counterUI.Counter.Add(changeValue);
        
        _saveProvider.SaveData.TrySave(counterUI.Counter.Type, counterUI.Counter.Count.Value);
        _saveProvider.CurrencyData.TrySave(counterUI.Counter.Type, counterUI.Counter.Count.Value);
        _saveProvider.Save();
        
        instance.transform
            .DOMove(counterUI.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                counterUI.UpdateText();
                DestroyImmediate(instance.gameObject);
            });
    }
}