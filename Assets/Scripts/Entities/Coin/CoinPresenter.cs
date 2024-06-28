using UnityEngine;
using UniRx;
using Zenject;

public class CoinPresenter : MonoBehaviour, ICollectablePresenter
{
    [SerializeField] private CoinData _data;
    
    [Inject] private ICollectableModel _model;
    [Inject] private IWalletService _walletService;

    private void Start()
    {
        _model.IsCollected.Subscribe(HandleDeath);
    }
    
    public void Collect()
    {
        _model.Collect();
    }

    private void HandleDeath(bool value)
    {
        if (value == false)
            return;

        _walletService.Earn(_data.CoinsAmount);
        DestroyImmediate(gameObject);
    }
}