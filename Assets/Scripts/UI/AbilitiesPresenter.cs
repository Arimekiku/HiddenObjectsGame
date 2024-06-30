using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPresenter : MonoBehaviour
{
    [SerializeField] private Button _magnetButton;
    [SerializeField] private Button _compassButton;
    [SerializeField] private CollectableUICounter _coinCounter;
    [SerializeField] private CollectableUICounter _starCounter;
    
    private void Awake()
    {
        _magnetButton.OnClickAsObservable().Subscribe(_ => OnMagnetButtonPressed()).AddTo(this);
        _compassButton.OnClickAsObservable().Subscribe(_ => OnCompassButtonPressed()).AddTo(this);
    }

    private void OnMagnetButtonPressed()
    {
        if (!_coinCounter.Counter.TryRemove(25))
            return;
        
        Debug.Log("Skill used");
    }

    private void OnCompassButtonPressed()
    {
        if (!_starCounter.Counter.TryRemove(25))
            return;
        
        Debug.Log("Skill used");
    }
}