using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : ILevelSpawner
{
    private readonly LevelSpawnData _levelSpawnData;
    private readonly IFactoryService _factory;

    private readonly GameObject _container;

    private LevelSpawner(LevelSpawnData levelSpawnData, IFactoryService factory)
    {
        _levelSpawnData = levelSpawnData;
        _factory = factory;

        _container = new GameObject("Entities");
    }

    public T SpawnAndPlaceEntity<T>(Bounds levelBounds) where T: MonoBehaviour, ICollectablePresenter
    {
        T instance = _factory.Create<T>();
        instance.transform.parent = _container.transform;

        Vector2 randomPoint = GetRandomPointInCollider(levelBounds);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _levelSpawnData.MaxInstanceScale);
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

            Collider2D[] collider = Physics2D.OverlapCircleAll(convertPosition, _levelSpawnData.MinRangeBetweenObjects);
            if (collider.Any(c => c.TryGetComponent(out ICollectableModel _)))
                continue;

            return convertPosition;
        }

        return Vector2.zero;
    }
}