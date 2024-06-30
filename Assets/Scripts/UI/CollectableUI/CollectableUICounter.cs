using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectableUICounter : MonoBehaviour
{
    [SerializeField] private CollectableType _type;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _image;

    public ICounter Counter => _counter;

    [Inject] private ICounter _counter;
    
    private void Awake()
    {
        _counter.Initialize(_type);
        
        UpdateText();
    }

    public void UpdateText()
    {
        _countText.text = _counter.Count.Value.ToString();
    }
}