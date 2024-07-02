using System;

[Serializable]
public class CounterSaveData
{
    public int Count;
    public int Id;

    public CounterSaveData(int id)
    {
        Id = id;
        Count = 0;
    }
}