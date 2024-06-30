using System;
using UniRx;
using UnityEngine;
using Zenject;

public class TapHandler : DisposableEntity, IInitializable
{
    [Inject] private CameraTracker _cameraTracker;
    
    private Vector2 _worldPointOnStartScroll;

    private IObservable<Touch> touchStream;

    public void Initialize()
    {
        touchStream = Observable.EveryUpdate()
            .Where(_ => Input.touchCount != 0)
            .Select(_ => Input.GetTouch(0));

        touchStream.Subscribe(ScrollCamera).AddTo(this);
        touchStream.Subscribe(CheckOnCollectable).AddTo(this);
    }
    
    private void ScrollCamera(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            _worldPointOnStartScroll = _cameraTracker.MainCamera.ScreenToWorldPoint(touch.position);
            return;
        }

        Vector2 touchMove = _cameraTracker.MainCamera.ScreenToWorldPoint(touch.position);
        Vector3 direction = _worldPointOnStartScroll - touchMove;

        _cameraTracker.MainCamera.transform.position += direction;
    }

    private void CheckOnCollectable(Touch touch)
    {
        if (touch.phase != TouchPhase.Began)
            return;
        
        Vector2 worldPosition = _cameraTracker.MainCamera.ScreenToWorldPoint(touch.position);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (raycastHit2D.collider == null)
            return;
        
        if (raycastHit2D.collider.TryGetComponent(out CollectablePresenter collectable))
            collectable.Model.Collect();
        
        if (raycastHit2D.collider.TryGetComponent(out ProducerPresenter producer)) 
            producer.Model.Collect();
    }
}