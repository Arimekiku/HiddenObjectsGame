using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelModel
{
    [Inject] private LevelSpawnData _data;
    [Inject] private ILevelSpawner _levelSpawner;

    private readonly List<HiddenObjectPresenter> _hiddenObjects;
    private readonly List<CoinPresenter> _coins;
    private readonly List<StarPresenter> _stars;

    public LevelModel()
    {
        _coins = new List<CoinPresenter>();
        _stars = new List<StarPresenter>();
        _hiddenObjects = new List<HiddenObjectPresenter>();
    }
    
    public void SpawnEntitiesInBounds(Bounds mapBounds)
    {
        for (int i = 0; i < _data.MaxSpawnNumber; i++)
        {
            HiddenObjectPresenter hiddenObject = _levelSpawner.SpawnAndPlaceEntity<HiddenObjectPresenter>(mapBounds);
            _hiddenObjects.Add(hiddenObject);
            
            CoinPresenter coin = _levelSpawner.SpawnAndPlaceEntity<CoinPresenter>(mapBounds);
            _coins.Add(coin);
            
            StarPresenter star = _levelSpawner.SpawnAndPlaceEntity<StarPresenter>(mapBounds);
            _stars.Add(star);
        }

        for (uint i = _data.MaxSpawnNumber; i >= _data.InitialSpawnNumber; i--)
        {
            DisableEntity(_hiddenObjects.First(o => o.gameObject.activeSelf).gameObject);
            DisableEntity(_coins.First(c => c.gameObject.activeSelf).gameObject);
            DisableEntity(_stars.First(s => s.gameObject.activeSelf).gameObject);
        }
    }

    private void DisableEntity(GameObject entity)
    {
        entity.gameObject.SetActive(false);
    }
}