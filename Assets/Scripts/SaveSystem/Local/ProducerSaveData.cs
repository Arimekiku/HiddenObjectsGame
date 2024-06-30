using System;
using UnityEngine;

[Serializable]
public class ProducerSaveData
{
    public Vector3 Position;
    public bool IsCollected;
    public int Id;
    
    public ProducerSaveData(Matrix4x4 moveMatrix, bool collected, int id)
    {
        Position = moveMatrix.GetPosition();
        
        IsCollected = collected;

        Id = id;
    }
}