using UniRx;
using UnityEngine;
using Zenject;

public class StarPresenter : MonoBehaviour, ICollectablePresenter
{
    [SerializeField] private StarData _data;

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

        _walletService.Earn(_walletService.Stars, _data.StarAmount);
        DestroyImmediate(gameObject);
    }
}