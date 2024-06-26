using System;
using ModestTree;
using UnityEngine;

public class TapInput : MonoBehaviour
{
    public event Action<IClickable> OnTouchPerformedEvent;

    public void FixedUpdate()
    {
        CheckTouch();
    }

    private void CheckTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch currentTouch = Input.GetTouch(0);
        if (currentTouch.phase != TouchPhase.Began)
            return;

        Vector2 touchPoint = Camera.main.ScreenToWorldPoint(currentTouch.position);

        CheckTouchOnClickable(touchPoint);
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