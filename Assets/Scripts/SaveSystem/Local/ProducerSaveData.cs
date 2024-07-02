using System;
using UnityEngine;

[Serializable]
public class ProducerSaveData
{
    public Vector3 Position;
    public bool IsCollected;
    public int UniqueId;
    public int CreateId;
    
    public ProducerSaveData(ProducerPresenter producer)
    {
        Position = producer.transform.position;

        IsCollected = producer.Model.IsCollected.Value;

        UniqueId = producer.UniqueId;
        CreateId = producer.CreateId;
    }
}