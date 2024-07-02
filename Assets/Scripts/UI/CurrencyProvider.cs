using UnityEngine;
using Zenject;

public class CurrencyProvider : MonoBehaviour
{
    [field: SerializeField] public CounterPresenter MoneyCounterPresenter { get; private set; }
    [field: SerializeField] public CounterPresenter StarCounterPresenter { get; private set; }

    [Inject] private LevelCurrencyData _currencyData;
    [Inject] private SpriteProvider _spriteProvider;
    [Inject] private SaveProvider _saveProvider;

    public void BuildCurrency()
    {
        int coinCode = _spriteProvider.GetCoinSprite().GetHashCode();
        MoneyCounterPresenter.Initialize(coinCode, _currencyData.CoinsAmount);
        _saveProvider.CurrencyData.TryAdd(MoneyCounterPresenter);

        int starCode = _spriteProvider.GetStarSprite().GetHashCode();
        StarCounterPresenter.Initialize(starCode, _currencyData.StarAmount);
        _saveProvider.CurrencyData.TryAdd(StarCounterPresenter);
    }
}