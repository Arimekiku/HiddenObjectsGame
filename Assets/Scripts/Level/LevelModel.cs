using UnityEngine;
using Zenject;

public class LevelModel
{
    [Inject] private SpawnData _data;
    [Inject] private ILevelSpawner _levelSpawner;

    public void SpawnEntitiesInBounds(Bounds mapBounds)
    {
        for (int i = 0; i < _data.SpawnNumber; i++)
        {
            _levelSpawner.SpawnAndPlaceEntity<HiddenObjectPresenter>(mapBounds);
            _levelSpawner.SpawnAndPlaceEntity<CoinPresenter>(mapBounds);
            _levelSpawner.SpawnAndPlaceEntity<StarPresenter>(mapBounds);
        }
    }
}