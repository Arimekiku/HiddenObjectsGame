using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesData", menuName = "SO/AbilitiesData", order = 0)]
public class AbilitiesData : ScriptableObject
{
    [field: SerializeField] public uint MagnetRadius { get; private set; }
    [field: SerializeField] public uint MagnetCost { get; private set; }
    [field: SerializeField] public uint CompassRadius { get; private set; }
    [field: SerializeField] public uint CompassCost { get; private set; }
}