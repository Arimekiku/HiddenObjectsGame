using System;

[Serializable]
public class CounterSaveData
{
    public int Count;
    public CollectableType Type;

    public CounterSaveData(CollectableType type)
    {
        Type = type;
        Count = 0;
    }
}