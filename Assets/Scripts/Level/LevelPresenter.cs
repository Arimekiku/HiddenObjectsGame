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
    [Inject] private SaveProvider _saveProvider;

    private void Awake()
    {
        _model.SetupLevel(_mapBounds.bounds);
        
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
        
        if (_model.IsCompleted)
        {
            _levelSwapper.LoadNextLevel();
            return;
        }
        
        _uiSpawner.CollectAnimation(collectable, 1);
    }
}