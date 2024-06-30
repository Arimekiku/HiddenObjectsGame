using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelSaveData
{
    public List<CounterSaveData> CountersData;
    public List<HiddenObjectSaveData> EntitiesData;
    public List<ProducerSaveData> ProducersData;

    public LevelSaveData()
    {
        CountersData = new List<CounterSaveData>();
        EntitiesData = new List<HiddenObjectSaveData>();
        ProducersData = new List<ProducerSaveData>();
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

    public void UpdateProducer(ProducerPresenter producer)
    {
        ProducerSaveData saveData = ProducersData.First(p => p.Id == producer.Id);
        saveData.IsCollected = producer.Model.IsCollected.Value;
    }
}