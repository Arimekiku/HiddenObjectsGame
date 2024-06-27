using System;
using UnityEngine;
using Zenject;

public class HiddenObject : MonoBehaviour, IClickable
{
    [Inject] private HiddenObjectData _data;

    private void Awake()
    {
        
    }

    public event Action<IClickable> OnClickEvent;
    
    public void Click()
    {
        OnClickEvent?.Invoke(this);
    }
}

public class HiddenObjectFactory : PlaceholderFactory<HiddenObjectData, HiddenObject> { }