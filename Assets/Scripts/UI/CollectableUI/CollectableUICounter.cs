using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectableUICounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _image;

    public ICounter Counter => _counter;
    public int Id => _id;
    
    [Inject] private int _id;
    [Inject] private ICounter _counter;
    
    private void Awake()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        _countText.text = _counter.Count.Value.ToString();
    }

    public void UpdateImage(Sprite image)
    {
        _image.sprite = image;
    }
}

public class CollectableCounterFactory : PlaceholderFactory<int, CollectableUICounter> { }