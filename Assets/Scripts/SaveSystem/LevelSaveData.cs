using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelSaveData
{
    public List<CounterSaveData> CountersData;
    public List<HiddenObjectSaveData> EntitiesData;

    public LevelSaveData()
    {
        CountersData = new List<CounterSaveData>();
        EntitiesData = new List<HiddenObjectSaveData>();
    }

    public void SaveOrOverwriteCounter(CollectableType type, int newData)
    {
        CounterSaveData saveData = CountersData.FirstOrDefault(c => c.Type == type);
        if (saveData == null)
        {
            saveData = new CounterSaveData(type);
            CountersData.Add(saveData);
        }
        
        saveData.Count = newData;
    }
}