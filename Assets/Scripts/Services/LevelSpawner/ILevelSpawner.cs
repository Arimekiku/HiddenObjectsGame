using UnityEngine;

public interface ILevelSpawner
{
    public CollectablePresenter SpawnAndPlaceCollectable(Bounds levelBounds, CollectableType type);
    public CollectablePresenter SpawnAndPlaceCollectable(HiddenObjectSaveData saveData);
    public ProducerPresenter SpawnAndPlaceProducer(Bounds levelBounds, bool isCollected);
    public ProducerPresenter SpawnAndPlaceProducer(ProducerSaveData saveData);
}