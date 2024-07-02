using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectableUIPresenter : MonoBehaviour
{
    [SerializeField] private Image _image;

    [Inject] private CollectablePresenter _presenter;

    private void Awake()
    {
        _image.sprite = _presenter.Model.Sprite.Value;
    }
}

public class CollectableUIFactory : PlaceholderFactory<CollectablePresenter, CollectableUIPresenter> { }