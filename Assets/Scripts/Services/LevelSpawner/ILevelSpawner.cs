using UnityEngine;

public interface ILevelSpawner
{
    public void SpawnAndPlaceEntity<T>(Bounds levelBounds) where T : MonoBehaviour, ICollectablePresenter;
}