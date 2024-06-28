using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AbilitiesPresenter : MonoBehaviour
{
    [SerializeField] private Button _magnetButton;
    [SerializeField] private Button _compassButton;

    [Inject] private IWalletService _wallet;

    private void Awake()
    {
        _magnetButton.OnClickAsObservable().Subscribe(_ => OnMagnetButtonPressed());
        _compassButton.OnClickAsObservable().Subscribe(_ => OnCompassButtonPressed());
    }

    private void OnMagnetButtonPressed()
    {
        if (!_wallet.TrySpend(_wallet.Coins, 25))
            return;
        
        Debug.Log("Skill used");
    }

    private void OnCompassButtonPressed()
    {
        if (!_wallet.TrySpend(_wallet.Coins, 25))
            return;
        
        Debug.Log("Skill used");
    }
}