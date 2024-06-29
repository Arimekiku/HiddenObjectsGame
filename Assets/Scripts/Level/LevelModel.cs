using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class LevelModel
{
    [Inject] private LevelSpawnData _data;
    [Inject] private ILevelSpawner _levelSpawner;

    private readonly CollectableType[] _hiddenObjectTypes;
    private readonly List<CollectablePresenter> _collectables;

    public LevelModel()
    {
        _collectables = new List<CollectablePresenter>();
        
        _hiddenObjectTypes = new CollectableType[]
        {
            CollectableType.Hammer,
            CollectableType.Steerwheel,
            CollectableType.Salt,
            CollectableType.Joystick,
            CollectableType.Kettle,
        };
    }
    
    public async void SpawnEntitiesInBounds(Bounds mapBounds)
    {
        for (int i = 0; i < _data.MaxSpawnNumber; i++)
        {
            CollectableType hiddenObjectType = _hiddenObjectTypes[Random.Range(0, _hiddenObjectTypes.Length)];
            CollectablePresenter hiddenObject = await _levelSpawner.SpawnAndPlaceEntity(mapBounds, hiddenObjectType);
            
            CollectablePresenter coin = await _levelSpawner.SpawnAndPlaceEntity(mapBounds, CollectableType.Coin);
            CollectablePresenter star = await _levelSpawner.SpawnAndPlaceEntity(mapBounds, CollectableType.Star);

            if (i >= _data.InitialSpawnNumber) 
                continue;
            
            hiddenObject.gameObject.SetActive(true);
            coin.gameObject.SetActive(true);
            star.gameObject.SetActive(true);
        }
    }
}