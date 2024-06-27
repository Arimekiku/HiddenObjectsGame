using UnityEngine;

[CreateAssetMenu(fileName = "CollectableData", menuName = "SO/CollectableData", order = 0)]
public class CollectableData : ScriptableObject
{
    [field: SerializeField] public uint DataId { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}