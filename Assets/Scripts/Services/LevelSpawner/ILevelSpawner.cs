using System.Threading.Tasks;
using UnityEngine;

public interface ILevelSpawner
{
    public Task<CollectablePresenter> SpawnAndPlaceEntity(Bounds levelBounds, CollectableType type);
}