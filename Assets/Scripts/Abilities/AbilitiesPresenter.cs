using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AbilitiesPresenter : MonoBehaviour
{
    [SerializeField] private Button _magnetButton;
    [SerializeField] private TextMeshProUGUI _magnetCost;
    [SerializeField] private Button _compassButton;
    [SerializeField] private TextMeshProUGUI _compassCost;
    [SerializeField] private CounterPresenter _coinCounterPresenter;
    [SerializeField] private CounterPresenter _starCounterPresenter;

    [Inject] private CameraTracker _tracker;
    [Inject] private AbilitiesData _data;
    
    private void Awake()
    {
        _magnetCost.text = _data.MagnetCost.ToString();
        _compassCost.text = _data.CompassCost.ToString();
        
        _magnetButton.OnClickAsObservable().Subscribe(_ => OnMagnetButtonPressed()).AddTo(this);
        _compassButton.OnClickAsObservable().Subscribe(_ => OnCompassButtonPressed()).AddTo(this);
    }

    private void OnMagnetButtonPressed()
    {
        if (!_coinCounterPresenter.CounterModel.TryRemove((int)_data.MagnetCost))
            return;

        RaycastHit2D[] rays = Physics2D.CircleCastAll(
            _tracker.MainCamera.transform.position, 
            _data.MagnetRadius, 
            Vector2.zero);

        foreach (RaycastHit2D hit in rays)
        {
            if (hit.collider.TryGetComponent(out CollectablePresenter collectable))
                collectable.Model.Collect();
            
            if (hit.collider.TryGetComponent(out ProducerPresenter producer))
                producer.Model.Collect();
        }
        
        _coinCounterPresenter.UpdateText();
    }

    private void OnCompassButtonPressed()
    {
        if (!_starCounterPresenter.CounterModel.TryRemove((int)_data.CompassCost))
            return;
        
        RaycastHit2D[] rays = Physics2D.CircleCastAll(
            _tracker.MainCamera.transform.position, 
            _data.CompassRadius, 
            Vector2.zero);
        
        foreach (RaycastHit2D hit in rays)
        {
            if (hit.collider.TryGetComponent(out CollectablePresenter collectable))
            {
                Vector2 cameraPosition = _tracker.MainCamera.transform.position;
                
                collectable.transform.position = cameraPosition;
                break;
            }
            
            if (hit.collider.TryGetComponent(out ProducerPresenter producer))
            {
                if (producer.Model.IsCollected.Value == true)
                    continue;
                
                Vector2 cameraPosition = _tracker.MainCamera.transform.position;
                
                producer.transform.position = cameraPosition;
                break;
            }
        }
        
        _starCounterPresenter.UpdateText();
    }
}