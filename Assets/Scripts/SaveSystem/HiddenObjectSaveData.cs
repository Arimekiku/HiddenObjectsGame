using System;
using UnityEngine;

[Serializable]
public class HiddenObjectSaveData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public CollectableType Type;

    public HiddenObjectSaveData(Matrix4x4 moveMatrix, CollectableType type)
    {
        Position = moveMatrix.GetPosition();
        Rotation = moveMatrix.rotation.eulerAngles;
        Scale = moveMatrix.lossyScale;
        
        Type = type;
    }
}