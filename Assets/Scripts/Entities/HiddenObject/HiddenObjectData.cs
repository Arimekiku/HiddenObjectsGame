using UnityEngine;

[CreateAssetMenu(fileName = "CollectableData", menuName = "SO/CollectableData", order = 0)]
public class HiddenObjectData : ScriptableObject
{
    [field: SerializeField] public Sprite[] Sprites { get; private set; }
}