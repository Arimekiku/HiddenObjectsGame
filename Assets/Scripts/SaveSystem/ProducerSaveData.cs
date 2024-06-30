using System;
using UnityEngine;

[Serializable]
public class ProducerSaveData
{
    public Vector3 Position;
    public bool IsCollected;
    
    public ProducerSaveData(Matrix4x4 moveMatrix, bool collected)
    {
        Position = moveMatrix.GetPosition();
        
        IsCollected = collected;
    }
}