using TMPro;
using UnityEngine;
using Zenject;

public class CurrencyPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoinsText;
    [SerializeField] private TextMeshProUGUI StarsText;
    
    [Inject] private IWalletService _walletService;

    private void Awake()
    {
        _walletService.Coins.SubscribeToText(CoinsText);
        _walletService.Stars.SubscribeToText(StarsText);
    }
}