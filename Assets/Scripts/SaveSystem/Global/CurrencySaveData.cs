using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CurrencySaveData
{
    public List<CounterSaveData> CountersData;

    public CurrencySaveData()
    {
        CountersData = new List<CounterSaveData>();
        foreach (CollectableType type in Constants.Currency)
            CountersData.Add(new CounterSaveData(type));
    }

    public bool TrySave(CollectableType type, int newData)
    {
        CounterSaveData saveData = CountersData.FirstOrDefault(c => c.Type == type);
        if (saveData == null)
            return false;
        
        saveData.Count = newData;
        return true;
    }
}