using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUIHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private CollectableType _type;
    [SerializeField] private Image _image;

    private int _count;

    private void Awake()
    {
        _countText.text = _count.ToString();
    }

    public void UpdateCount(int value)
    {
        _count = value;
        _countText.text = _count.ToString();
    }
}