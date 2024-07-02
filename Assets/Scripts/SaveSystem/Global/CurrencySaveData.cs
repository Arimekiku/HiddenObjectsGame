using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CurrencySaveData
{
    public List<CounterSaveData> CountersData;

    public CurrencySaveData(List<CollectableUICounter> counters)
    {
        CountersData = new List<CounterSaveData>();
        foreach (CollectableUICounter counter in counters)
            CountersData.Add(new CounterSaveData(counter.Id));
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