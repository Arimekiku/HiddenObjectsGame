using UnityEngine;

[CreateAssetMenu(fileName = "HiddenObjectData", menuName = "SO/HiddenObjectData", order = 0)]
public class HiddenObjectData : ScriptableObject
{
    [field: SerializeField] public uint Id { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}