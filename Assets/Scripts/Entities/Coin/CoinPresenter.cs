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
        _model.IsCollected.Subscribe(_ => _walletService.Earn(_data.CoinsAmount));
    }

    private void HandleDeath(bool value)
    {
        if (value == false)
            return;

        DestroyImmediate(gameObject);
    }

    private void OnMouseDown()
    {
        _model.Collect();
    }

}