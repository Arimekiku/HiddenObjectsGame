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

        LoadLevel(_levelSwapper.LoadCurrentLevel());
    }

    private void OnCollectableClicked(CollectablePresenter collectable)
    {
        if (_model.IsCompleted)
        {
            Destroy(collectable.gameObject);
            StartNextLevel();
            return;
        }
        
        _uiSpawner.CollectAnimation(collectable);
        collectable.gameObject.SetActive(false);
    }

    private void StartNextLevel()
    {
        _model.ClearLevel();
        _uiSpawner.ClearCounters();
        
        LoadLevel(_levelSwapper.LoadNextLevel());
    }

    private void LoadLevel(LevelData data)
    {
        _model.SetupLevel(transform, data);
        _uiSpawner.CreateCounters(_model.HiddenObjects.ToList(), _model.Producers.ToList());

        _saveProvider.Save();
        _model.OnCollectableClicked.Subscribe(OnCollectableClicked);
    }
}