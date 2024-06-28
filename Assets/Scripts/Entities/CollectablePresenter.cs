using UniRx;
using UnityEngine;
using Zenject;

public class CollectablePresenter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    
    [Inject] private ICollectableModel _model;

    private void Awake()
    {
        _renderer.sprite = _model.Data.Sprite;
    }
    
    private void Start()
    {
        _model.IsCollected.Subscribe(HandleDeath);
    }

    private void HandleDeath(bool value)
    {
        if (value == false)
            return;
        
        DestroyImmediate(gameObject);
    }

    private void OnMouseDown()
    {
        _model.Collect();
    }
}

public class CollectableFactory : PlaceholderFactory<ICollectableModel, CollectablePresenter> { }