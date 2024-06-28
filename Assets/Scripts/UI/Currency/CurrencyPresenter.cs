using TMPro;
using UnityEngine;
using Zenject;

public class CurrencyPresenter : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI CoinsText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI StarsText { get; private set; }

    [Inject] private IWalletService _walletService;

    private void Awake()
    {
        _walletService.Coins.SubscribeToText(CoinsText);
        _walletService.Stars.SubscribeToText(StarsText);
    }
}