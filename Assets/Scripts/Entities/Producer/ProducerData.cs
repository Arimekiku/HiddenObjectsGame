using UnityEngine;

[CreateAssetMenu(fileName = "ProducerData", menuName = "SO/ProducerData", order = 0)]
public class ProducerData : ScriptableObject
{
    [field: SerializeField] public uint SpawnRadius { get; private set; }
}