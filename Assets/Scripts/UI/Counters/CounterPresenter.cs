using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CounterPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _image;

    public ICounterModel CounterModel => _counterModel;

    [Inject] private ICounterModel _counterModel;
    
    private void Awake()
    {
        UpdateText();

        _counterModel.Position.Subscribe(HandlePosition);
    }

    public void Initialize(int id, int delta = 1)
    {
        _counterModel.Initialize(transform.position, id, delta);
    }

    public void UpdateText()
    {
        _countText.text = _counterModel.Count.Value.ToString();
    }

    public void UpdateImage(Sprite image)
    {
        _image.sprite = image;
    }

    private void HandlePosition(Vector3 position)
    {
        transform.position = position;
    }
}

public class CollectableCounterFactory : PlaceholderFactory<CounterPresenter> { }