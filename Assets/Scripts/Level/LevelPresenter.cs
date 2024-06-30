using UniRx;
using UnityEngine;
using Zenject;

public class LevelPresenter : MonoBehaviour
{
    [Inject] private LevelCurrencyData _currencyData;
    [Inject] private LevelModel _model;
    [Inject] private CounterProvider _uiSpawner;
    [Inject] private SaveProvider _saveProvider;

    private void Awake()
    {
        _model.SetupLevel(transform);
        
        _model.OnCollectableClicked.Subscribe(OnCollectableClicked);
    }

    private void OnCollectableClicked(CollectablePresenter collectable)
    {
        collectable.gameObject.SetActive(false);

        if (collectable.Model.Type == CollectableType.Coin)
        {
            _uiSpawner.CollectAnimation(collectable, _currencyData.CoinsAmount);
            return;
        }

        if (collectable.Model.Type == CollectableType.Star)
        {
            _uiSpawner.CollectAnimation(collectable, _currencyData.StarAmount);
            return;
        }
        
        _uiSpawner.CollectAnimation(collectable, 1);
    }
}