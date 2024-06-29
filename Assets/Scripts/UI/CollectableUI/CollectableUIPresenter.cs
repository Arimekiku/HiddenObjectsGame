using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUIPresenter : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private CollectableType _type;
    [SerializeField] private Image _renderer;

    private CollectableUIModel _collectableUIModel;

    private void Awake()
    {
        _collectableUIModel = new CollectableUIModel();

        _collectableUIModel.Sprite.Subscribe(UpdateSprite);
        
        _collectableUIModel.Initialize(_type, _sprite);
    }

    private void UpdateSprite(Sprite newSprite)
    {
        _renderer.sprite = newSprite;
    }
}