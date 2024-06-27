using System;
using UnityEngine;

public class HiddenObjectProducer : MonoBehaviour, ICollectableModel
{
    [SerializeField] private CollectableData _data;
    
    public event Action<ICollectableModel> OnClickEvent;
    
    public void Collect()
    {
        OnClickEvent.Invoke(this);
    }
}