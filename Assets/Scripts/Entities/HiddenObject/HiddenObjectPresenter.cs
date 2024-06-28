using UniRx;
using UnityEngine;
using Zenject;

public class HiddenObjectPresenter : MonoBehaviour, ICollectablePresenter
{
    [SerializeField] private SpriteRenderer _renderer;
    
    [Inject] private ICollectableModel _model;
    [Inject] private ISpriteProvider _spriteProvider;
    
    private async void Awake()
    {
        Sprite randomSprite = await _spriteProvider.Load("ItemSheet[joystick]");
        _renderer.sprite = randomSprite;
    }
    
    private void Start()
    {
        _model.IsCollected.Subscribe(HandleDeath);
    }
    
    public void Collect()
    {
        _model.Collect();
    }

    private void HandleDeath(bool value)
    {
        if (value == false)
            return;
        
        DestroyImmediate(gameObject);
    }
}