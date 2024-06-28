using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "SO/CoinData", order = 0)]
public class CoinData : ScriptableObject
{
    [field: SerializeField] public int CoinsAmount { get; private set; }
}