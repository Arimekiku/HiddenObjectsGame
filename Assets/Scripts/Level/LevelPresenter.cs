using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelPresenter : MonoBehaviour
{
    [Inject] private LevelCurrencyData _currencyData;
    [Inject] private LevelModel _model;
    [Inject] private CounterProvider _uiSpawner;
    [Inject] private ILevelSwapper _levelSwapper;
    [Inject] private ISpriteProvider _spriteProvider;
    [Inject] private SaveProvider _saveProvider;
    [Inject] private CurrencyProvider _currencyProvider;

    private async void Awake()
    {
        await _spriteProvider.LoadSprites();
        
        if (_saveProvider.TryLoad() == false)
        {
            _saveProvider.SaveData = new LevelSaveData();
            _saveProvider.CurrencyData = new CurrencySaveData(new List<CollectableUICounter>());
        }
        
        _model.SetupLevel(transform);
        _uiSpawner.CreateCounters(_model.HiddenObjects.ToList(), _model.Producers.ToList());
        
        _model.OnCollectableClicked.Subscribe(OnCollectableClicked);
    }

    private void OnCollectableClicked(CollectablePresenter collectable)
    {
        if (_model.IsCompleted)
        {
            _model.Dispose();
            _saveProvider.SaveData.ClearData();
            _saveProvider.Save();
            
            _levelSwapper.LoadNextLevel();
            return;
        }
        
        _uiSpawner.CollectAnimation(collectable);
        _saveProvider.Save();
        collectable.gameObject.SetActive(false);
    }
}