using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelSaveData
{
    public int LevelIndex;
    public List<CounterSaveData> CountersData;
    public List<HiddenObjectSaveData> EntitiesData;
    public List<ProducerSaveData> ProducersData;

    public LevelSaveData(int levelIndex)
    {
        LevelIndex = levelIndex;
        
        CountersData = new List<CounterSaveData>();
        EntitiesData = new List<HiddenObjectSaveData>();
        ProducersData = new List<ProducerSaveData>();
    }

    public void UpdateIndex(int index)
    {
        LevelIndex = index;
    }
    
    public void AddCounter(int id)
    {
        if (CountersData.Any(c => c.Id == id))
            return;
        
        CountersData.Add(new CounterSaveData(id));
    }

    public bool UpdateCounter(int id, int newData)
    {
        var saveData = CountersData.FirstOrDefault(c => c.Id == id);
        
        if (saveData == null)
            return false;
        
        saveData.Count = newData;
        return true;
    }

    public void AddProducer(ProducerPresenter producer)
    {
        if (ProducersData.Any(p => p.UniqueId == producer.UniqueId))
            return;
        
        ProducersData.Add(new ProducerSaveData(producer));
    }
    
    public void UpdateProducer(ProducerPresenter producer)
    {
        var saveData = ProducersData.First(p => p.UniqueId == producer.UniqueId);
        saveData.IsCollected = producer.Model.IsCollected.Value;
    }
    
    public void ClearData()
    {
        EntitiesData.Clear();
        CountersData.Clear();
        ProducersData.Clear();
    }
}