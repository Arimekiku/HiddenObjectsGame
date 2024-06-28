using UnityEngine;

[CreateAssetMenu(fileName = "StarData", menuName = "SO/StarData", order = 0)]
public class StarData : CollectableData
{
    [field: SerializeField] public int StarAmount { get; private set; }
}