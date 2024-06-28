using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : IDisposable
{
    private const string HAMMER_DATA_PATH = "SO/HiddenObjects/Hammer";
    private const string KETTLE_DATA_PATH = "SO/HiddenObjects/Kettle";
    private const string SALT_DATA_PATH = "SO/HiddenObjects/Salt";
    private const string JOYSTICK_DATA_PATH = "SO/HiddenObjects/Joystick";
    private const string STEERWHEEL_DATA_PATH = "SO/HiddenObjects/Steerwheel";
    private const string COIN_DATA_PATH = "SO/CoinData";
    private const string STAR_DATA_PATH = "SO/StarData";
    
    private readonly SpawnData _spawnData;
    private readonly CollectableFactory _factory;
    private readonly List<CollectableData> _datas;
    private readonly CoinData _coinData;
    private readonly StarData _starData;

    private LevelSpawner(SpawnData spawnData, CollectableFactory factory)
    {
        _spawnData = spawnData;
        _factory = factory;
        
        _datas = new List<CollectableData>
        {
            Resources.Load<CollectableData>(HAMMER_DATA_PATH),
            Resources.Load<CollectableData>(KETTLE_DATA_PATH),
            Resources.Load<CollectableData>(SALT_DATA_PATH),
            Resources.Load<CollectableData>(JOYSTICK_DATA_PATH),
            Resources.Load<CollectableData>(STEERWHEEL_DATA_PATH),
        };

        _coinData = Resources.Load<CoinData>(COIN_DATA_PATH);
        _starData = Resources.Load<StarData>(STAR_DATA_PATH);
    }
    
    public void Dispose()
    {
        _datas.Clear();
    }

    public void SpawnAndPlaceHiddenObject(Bounds levelBounds)
    {
        ICollectableModel newModel = new CollectableModel(_datas[Random.Range(0, _datas.Count)]);
        CollectablePresenter instance = _factory.Create(newModel);

        PlaceEntity(instance, levelBounds);
    }
    
    public void SpawnAndPlaceCoin(Bounds levelBounds)
    {
        ICollectableModel newModel = new CollectableModel(_coinData);
        CollectablePresenter instance = _factory.Create(newModel);
        
        PlaceEntity(instance, levelBounds);
    }
    
    public void SpawnAndPlaceStar(Bounds levelBounds)
    {
        ICollectableModel newModel = new CollectableModel(_starData);
        CollectablePresenter instance = _factory.Create(newModel);
        
        PlaceEntity(instance, levelBounds);
    }

    private void PlaceEntity(CollectablePresenter entity, Bounds levelBounds)
    {
        Vector2 randomPoint = GetRandomPointInCollider(levelBounds);
        entity.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _spawnData.MaxInstanceScale);
        entity.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        entity.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
    }
    
    private Vector2 GetRandomPointInCollider(Bounds mapBounds)
    {
        for (int i = 0; i < 200; i++)
        {
            float xComponent = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float yComponent = Random.Range(mapBounds.min.y, mapBounds.max.y);
            Vector2 convertPosition = new Vector2(xComponent, yComponent);

            Collider2D[] collider = Physics2D.OverlapCircleAll(convertPosition, _spawnData.MinRangeBetweenObjects);
            if (collider.Any(c => c.TryGetComponent(out ICollectableModel _)))
                continue;

            return convertPosition;
        }

        return Vector2.zero;
    }
}