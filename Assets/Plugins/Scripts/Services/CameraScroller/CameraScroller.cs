using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class CameraScroller : MonoBehaviour
{
    [Inject] private CameraTracker _cameraTracker;

    private Vector2 _worldPointOnStartScroll;
    
    private void Awake()
    {
        this.UpdateAsObservable()
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