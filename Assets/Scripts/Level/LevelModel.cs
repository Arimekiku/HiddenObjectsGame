using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelModel
{
    [Inject] private LevelSpawnData _data;
    [Inject] private ILevelSpawner _levelSpawner;

    private readonly CollectableType[] _hiddenObjectTypes;

    public IReadOnlyList<CollectablePresenter> Coins =>
        _levelSpawner.Collectables.Where(c => c.Model.Type == CollectableType.Coin).ToList();

    public IReadOnlyList<CollectablePresenter> Stars =>
        _levelSpawner.Collectables.Where(c => c.Model.Type == CollectableType.Star).ToList();

    public IReadOnlyList<CollectablePresenter> HiddenObjects =>
        _levelSpawner.Collectables.Where(c => _hiddenObjectTypes.Any(t => t == c.Model.Type)).ToList();

    public LevelModel()
    {
        _hiddenObjectTypes = new CollectableType[]
        {
            CollectableType.Hammer,
            CollectableType.Steerwheel,
            CollectableType.Salt,
            CollectableType.Joystick,
            CollectableType.Kettle,
        };
    }

    public void SpawnEntitiesInBounds(Bounds mapBounds)
    {
        for (int i = 0; i < _data.MaxSpawnNumber; i++)
        {
            CollectableType hiddenObjectType = _hiddenObjectTypes[Random.Range(0, _hiddenObjectTypes.Length)];
            _levelSpawner.SpawnAndPlaceEntity(mapBounds, hiddenObjectType);

            _levelSpawner.SpawnAndPlaceEntity(mapBounds, CollectableType.Coin);
            _levelSpawner.SpawnAndPlaceEntity(mapBounds, CollectableType.Star);
        }

        for (int i = 0; i < _data.InitialSpawnNumber; i++)
        {
            CollectablePresenter hidden = GetCollectableOfType(_hiddenObjectTypes);
            hidden.gameObject.SetActive(true);

            CollectablePresenter coin = GetCollectableOfType(CollectableType.Coin);
            coin.gameObject.SetActive(true);

            CollectablePresenter star = GetCollectableOfType(CollectableType.Star);
            star.gameObject.SetActive(true);
        }

        foreach (CollectablePresenter collectable in _levelSpawner.Collectables)
            collectable.Model.OnCollect.Subscribe(_ => _levelSpawner.RemoveEntity(collectable));
    }

    private CollectablePresenter GetCollectableOfType(params CollectableType[] types)
    {
        foreach (CollectablePresenter collectable in _levelSpawner.Collectables)
        {
            if (collectable.gameObject.activeSelf == true)
                continue;

            if (types.Any(t => t == collectable.Model.Type))
                return collectable;
        }

        throw new Exception("Can't process collectable request");
    }
}