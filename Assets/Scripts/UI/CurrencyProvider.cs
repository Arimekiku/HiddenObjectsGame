using UnityEngine;

public class CurrencyProvider : MonoBehaviour
{
    [field: SerializeField] public CollectableUICounter MoneyCounter { get; private set; }
    [field: SerializeField] public CollectableUICounter StarCounter { get; private set; }
}