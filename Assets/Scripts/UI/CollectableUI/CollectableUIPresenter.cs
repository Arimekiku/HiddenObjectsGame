using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectableUIPresenter : MonoBehaviour
{
    [SerializeField] private CollectableType _type;
    [SerializeField] private Image _image;

    [Inject] private CollectableModel _model;

    private void Awake()
    {
        _type = _model.Type;
        _image.sprite = _model.Sprite.Value;
    }
}

public class CollectableUIFactory : PlaceholderFactory<CollectableModel, CollectableUIPresenter> { }