using UnityEngine;

public interface ILevelSpawner
{
    public CollectablePresenter SpawnAndPlaceCollectable(uint radius, Transform center, Sprite sprite);
    public CollectablePresenter SpawnAndPlaceCollectable(HiddenObjectSaveData saveData);
    public ProducerPresenter SpawnAndPlaceProducer(uint radius, Transform center);
    public ProducerPresenter SpawnAndPlaceProducer(ProducerSaveData saveData);
}