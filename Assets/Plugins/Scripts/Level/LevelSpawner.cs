using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner
{
    private readonly SpawnData _spawnData;
    private readonly MonoFactory _factory;

    public LevelSpawner(MonoFactory factory, SpawnData spawnData)
    {
        _factory = factory;
        
        _spawnData = spawnData;
    }

    public T SpawnAndPlaceEntity<T>(BoxCollider2D levelCollider) where T : MonoBehaviour, IClickable
    {
        Vector2 randomPoint = GetRandomPointInCollider(levelCollider.bounds);

        T instance = _factory.SpawnEntity<T>();
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