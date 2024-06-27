using UniRx;
using UnityEngine;
using Zenject;

[SelectionBase]
public class HiddenObjectPresenter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer; 
    
    [Inject] private CollectableData _data;
    [Inject] private CollectableModel _model;

    private void Awake()
    {
        _renderer.sprite = _data.Sprite;
        
        _model.IsCollected.Subscribe(HandleDeath);
    }

    private void HandleDeath(bool isDead)
    {
        if (isDead == false)
            return;
        
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        _model.Collect();
    }
}

public class HiddenObjectFactory : PlaceholderFactory<CollectableData, HiddenObjectPresenter> { }