using UnityEngine;

public interface ILevelSpawner
{
    public T SpawnAndPlaceEntity<T>(Bounds levelBounds) where T : MonoBehaviour, ICollectablePresenter;
}