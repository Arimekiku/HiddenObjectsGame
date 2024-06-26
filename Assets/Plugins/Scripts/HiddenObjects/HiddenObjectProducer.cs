using System;
using UnityEngine;

public class HiddenObjectProducer : MonoBehaviour, IClickable
{
    [SerializeField] private HiddenObjectData _data;
    
    public event Action<IClickable> OnClickEvent;
    
    public void Click()
    {
        OnClickEvent.Invoke(this);
    }
}