using UnityEngine;

public interface ILevelSpawner
{
    public CollectablePresenter SpawnAndPlaceEntity(Bounds levelBounds, CollectableType type);
    public CollectablePresenter SpawnAndPlaceEntity(HiddenObjectSaveData saveData);
}