using System;
using UnityEngine;

public class HiddenObject : MonoBehaviour, IClickable
{
    [SerializeField] private HiddenObjectData _data;
    
    public event Action<IClickable> OnClickEvent;
    
    public void Click()
    {
        OnClickEvent?.Invoke(this);
    }
}