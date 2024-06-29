using UnityEngine;

public class CurrencyPresenter : MonoBehaviour
{
    [SerializeField] private CollectableUIHolder CoinsHolder;
    [SerializeField] private CollectableUIHolder StarsHolder;
    
    public void UpdateCoinsAnimation(int count)
    {
        CoinsHolder.UpdateCount(count);
    }

    public void UpdateStarsAnimation(int count)
    {
        StarsHolder.UpdateCount(count);
    }
}