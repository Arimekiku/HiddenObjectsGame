using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : ILevelSpawner
{
    private readonly SpawnData _spawnData;
    private readonly IFactoryService _factory;

    private LevelSpawner(SpawnData spawnData, IFactoryService factory)
    {
        _spawnData = spawnData;
        _factory = factory;
    }

    public void SpawnAndPlaceEntity<T>(Bounds levelBounds) where T: MonoBehaviour, ICollectablePresenter
    {
        T instance = _factory.Create<T>();

        Vector2 randomPoint = GetRandomPointInCollider(levelBounds);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _spawnData.MaxInstanceScale);
        instance.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        instance.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
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