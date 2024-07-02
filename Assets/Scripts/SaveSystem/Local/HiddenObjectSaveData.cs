using System;
using UnityEngine;

[Serializable]
public class HiddenObjectSaveData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public bool IsEnabled;
    public int UniqueId;
    public int SpriteId;

    public HiddenObjectSaveData(CollectablePresenter collectable)
    {
        Position = collectable.transform.position;
        Rotation = collectable.transform.rotation.eulerAngles;
        Scale = collectable.transform.lossyScale;

        IsEnabled = collectable.gameObject.activeSelf;
        
        UniqueId = collectable.UniqueId;
        SpriteId = collectable.Model.Sprite.Value.GetHashCode();
    }
}