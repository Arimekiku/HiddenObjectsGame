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
        foreach (CollectableType type in Constants.HiddenObjects)
            CountersData.Add(new CounterSaveData(type));

        EntitiesData = new List<HiddenObjectSaveData>();
        ProducersData = new List<ProducerSaveData>();
    }

    public bool TrySave(CollectableType type, int newData)
    {
        CounterSaveData saveData = CountersData.FirstOrDefault(c => c.Type == type);
        
        if (saveData == null)
            return false;
        
        saveData.Count = newData;
        return true;
    }
    
    public void ClearData()
    {
        foreach (CounterSaveData data in CountersData)
            data.Count = 0;

        ProducersData.Clear();
    }

    public void UpdateProducer(ProducerPresenter producer)
    {
        ProducerSaveData saveData = ProducersData.First(p => p.Id == producer.Id);
        saveData.IsCollected = producer.Model.IsCollected.Value;
    }
}