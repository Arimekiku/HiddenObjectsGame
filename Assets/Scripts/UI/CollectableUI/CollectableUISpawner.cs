using DG.Tweening;
using UnityEngine;
using Zenject;

public class CollectableUISpawner : MonoBehaviour
{
    [SerializeField] private CollectableUIPresenter _collectableUIPrefab;
    [SerializeField] private Canvas _parent;
    [SerializeField] private CollectableUIHolder _holder;

    [Inject] private CurrencyPresenter _currencyPresenter;
    [Inject] private IWalletService _walletService;

    private void Awake()
    {
        TestCollectAnimation();
    }

    private void TestCollectAnimation()
    {
        CollectableUIPresenter instance = Instantiate(_collectableUIPrefab, _parent.transform);

        instance.transform
            .DOMove(_holder.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .SetAutoKill(true)
            .OnComplete(() =>
            {
                _currencyPresenter.UpdateCoinsAnimation(_walletService.Coins.Value);
                DestroyImmediate(instance);
            });
    }
}