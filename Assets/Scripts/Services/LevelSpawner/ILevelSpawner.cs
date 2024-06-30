using UnityEngine;

public interface ILevelSpawner
{
    public CollectablePresenter SpawnAndPlaceCollectable(uint radius, Transform center, CollectableType type);
    public CollectablePresenter SpawnAndPlaceCollectable(HiddenObjectSaveData saveData);
    public ProducerPresenter SpawnAndPlaceProducer(uint radius, Transform center, bool isCollected);
    public ProducerPresenter SpawnAndPlaceProducer(ProducerSaveData saveData);
}