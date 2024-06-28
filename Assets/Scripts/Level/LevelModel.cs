using UnityEngine;
using Zenject;

public class LevelModel
{
    [Inject] private SpawnData _data;
    [Inject] private CameraScroller _cameraScroller;
    [Inject] private LevelSpawner _levelSpawner;

    public void SpawnEntitiesInBounds(Bounds mapBounds)
    {
        for (int i = 0; i < _data.SpawnNumber; i++)
        {
            _levelSpawner.SpawnAndPlaceHiddenObject(mapBounds);
            _levelSpawner.SpawnAndPlaceCoin(mapBounds);
            _levelSpawner.SpawnAndPlaceStar(mapBounds);
        }
    }
}