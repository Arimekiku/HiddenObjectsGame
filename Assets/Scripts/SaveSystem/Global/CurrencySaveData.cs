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

    public bool TryAdd(CounterPresenter counterPresenter)
    {
        int counterId = counterPresenter.CounterModel.Id.Value;
        
        if (CountersData.Any(c => c.Id == counterId))
            return false;
        
        CountersData.Add(new CounterSaveData(counterId));
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