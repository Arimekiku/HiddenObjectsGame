using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "SO/CurrencyData", order = 0)]
public class LevelCurrencyData : ScriptableObject
{
    [field: SerializeField] public int CoinsAmount { get; private set; }
    [field: SerializeField] public int StarAmount { get; private set; }
}