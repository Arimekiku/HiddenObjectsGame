using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner
{
    private const string SQUARE_DATA_PATH = "SO/HiddenObjects/Square";
    
    private readonly SpawnData _spawnData;
    private readonly HiddenObjectFactory _factory;
    private readonly List<HiddenObjectData> _datas;

    public LevelSpawner(HiddenObjectFactory factory, SpawnData spawnData)
    {
        _factory = factory;
        
        _spawnData = spawnData;

        _datas = new List<HiddenObjectData>
        {
            Resources.Load<HiddenObjectData>(SQUARE_DATA_PATH),
        };
    }

    public HiddenObject SpawnAndPlaceHiddenObject(BoxCollider2D levelCollider)
    {
        Vector2 randomPoint = GetRandomPointInCollider(levelCollider.bounds);

        HiddenObjectData randomData = _datas[Random.Range(0, _datas.Count)];
        HiddenObject instance = _factory.Create(randomData);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _spawnData.MaxInstanceScale);
        instance.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        instance.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        
        return instance;
    }
    
    private Vector2 GetRandomPointInCollider(Bounds mapBounds)
    {
        for (int i = 0; i < 200; i++)
        {
            float xComponent = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float yComponent = Random.Range(mapBounds.min.y, mapBounds.max.y);
            Vector2 convertPosition = new Vector2(xComponent, yComponent);

            Collider2D[] collider = Physics2D.OverlapCircleAll(convertPosition, _spawnData.MinRangeBetweenObjects);
            if (collider.Any(c => c.TryGetComponent(out IClickable _)))
                continue;

            return convertPosition;
        }

        return Vector2.zero;
    }
}