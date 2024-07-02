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
    }

    public bool TryAdd(CollectableUICounter counter)
    {
        if (CountersData.Any(c => c.Id == counter.Id))
            return false;
        
        CountersData.Add(new CounterSaveData(counter.Id));
        return true;
    }

    public bool TrySave(int id, int newData)
    {
        CounterSaveData saveData = CountersData.FirstOrDefault(c => c.Id == id);
        if (saveData == null)
            return false;
        
        saveData.Count = newData;
        return true;
    }
}