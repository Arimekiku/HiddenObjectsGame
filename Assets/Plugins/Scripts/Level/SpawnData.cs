using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SO/LevelData", order = 0)]
public class SpawnData : ScriptableObject
{
    [field: SerializeField] public uint SpawnNumber { get; private set; }
    [field: SerializeField] public uint MaxInstanceScale { get; private set; }
    [field: SerializeField] public uint MinRangeBetweenObjects { get; private set; }
    
}