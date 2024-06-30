using System.Collections.Generic;
using UnityEngine;

public interface ILevelSpawner
{
    public IReadOnlyList<CollectablePresenter> Collectables { get; }
    
    public void SpawnAndPlaceEntity(Bounds levelBounds, CollectableType type);
    public void RemoveEntity(CollectablePresenter entity);
}