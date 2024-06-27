using System;
using ModestTree;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class TapInput : MonoBehaviour
{
    public event Action<IClickable> OnTouchPerformedEvent;

    private void Awake()
    {
        var tapStream = this.UpdateAsObservable()
            .Where(_ => Input.touchCount != 0)
            .Select(_ => Input.GetTouch(0));
            
        tapStream.Where(x => x.phase == TouchPhase.Began)
            .Subscribe(x =>
            {
                Vector2 touchPoint = Camera.main.ScreenToWorldPoint(x.position);
                CheckTouchOnClickable(touchPoint);
            });
    }

    private void CheckTouchOnClickable(Vector2 position)
    {
        RaycastHit2D[] raycastAll = Physics2D.RaycastAll(position, Vector2.zero);
        if (raycastAll.IsEmpty())
            return;

        foreach (RaycastHit2D raycast in raycastAll)
        {
            if (!raycast.collider.TryGetComponent(out IClickable clickable)) 
                continue;
            
            OnTouchPerformedEvent.Invoke(clickable);
            return;
        }
    }
}