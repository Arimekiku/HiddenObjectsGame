using UniRx;
using UnityEngine;
using Zenject;

public class HiddenObjectPresenter : MonoBehaviour, ICollectablePresenter
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private HiddenObjectData _data;
    
    [Inject] private ICollectableModel _model;
    
    private void Awake()
    {
        Sprite randomSprite = _data.Sprites[Random.Range(0, _data.Sprites.Length)];
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