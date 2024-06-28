using UniRx;
using UnityEngine;
using Zenject;

public class TapHandler : IInitializable
{
    private readonly CameraTracker _cameraTracker;
    
    private Vector2 _worldPointOnStartScroll;

    public TapHandler(CameraTracker tracker)
    {
        _cameraTracker = tracker;
    }

    public void Initialize()
    {
        var touchStream = Observable.EveryUpdate()
            .Where(_ => Input.touchCount != 0)
            .Select(_ => Input.GetTouch(0));

        touchStream.Subscribe(ScrollCamera);
        touchStream.Subscribe(CheckOnCollectable);
    }
    
    private void ScrollCamera(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            _worldPointOnStartScroll = Camera.main.ScreenToWorldPoint(touch.position);
            return;
        }

        Vector2 touchMove = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 direction = _worldPointOnStartScroll - touchMove;

        _cameraTracker.transform.position += direction;
    }

    private void CheckOnCollectable(Touch touch)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (!raycastHit2D.collider.TryGetComponent(out ICollectablePresenter collectable)) 
            return;
        
        collectable.Collect();
    }
}