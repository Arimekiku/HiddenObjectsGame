using UnityEngine;

[CreateAssetMenu(fileName = "StarData", menuName = "SO/StarData", order = 0)]
public class StarData : ScriptableObject
{
    [field: SerializeField] public int StarAmount { get; private set; }
}