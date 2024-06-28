using UniRx;
using UnityEngine;
using Zenject;

public class CameraScroller : IInitializable
{
    private readonly CameraTracker _cameraTracker;
    
    private Vector2 _worldPointOnStartScroll;

    public CameraScroller(CameraTracker tracker)
    {
        _cameraTracker = tracker;
    }
    
    public void Initialize()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.touchCount != 0)
            .Select(_ => Input.GetTouch(0))
            .Subscribe(ScrollCamera);
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
}