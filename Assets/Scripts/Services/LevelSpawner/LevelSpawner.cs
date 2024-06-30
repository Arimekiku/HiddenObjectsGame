using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : ILevelSpawner
{
    public IReadOnlyList<CollectablePresenter> Collectables => _collectables;
    
    private readonly LevelSpawnData _levelSpawnData;
    private readonly CollectableFactory _factory;
    private readonly ISpriteProvider _spriteProvider;

    private readonly Dictionary<CollectableType, Sprite> CollectableTypeToSprite;
    
    private List<CollectablePresenter> _collectables;

    private LevelSpawner(LevelSpawnData levelSpawnData, CollectableFactory factory, ISpriteProvider spriteProvider)
    {
        _levelSpawnData = levelSpawnData;
        _factory = factory;
        _spriteProvider = spriteProvider;

        _collectables = new List<CollectablePresenter>();

        CollectableTypeToSprite = new Dictionary<CollectableType, Sprite> { { CollectableType.Empty, null } };
    }

    public async void SpawnAndPlaceEntity(Bounds levelBounds, CollectableType type)
    {
        CollectablePresenter instance = _factory.Create(type);
        _collectables.Add(instance);

        Vector2 randomPoint = GetRandomPointInCollider(levelBounds);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _levelSpawnData.MaxInstanceScale);
        instance.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        instance.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        
        Sprite sprite = await LoadSprite(type);
        instance.Model.UpdateSprite(sprite);
    }

    public void RemoveEntity(CollectablePresenter entity)
    {
        _collectables.Remove(entity);
    }
    
    private Vector2 GetRandomPointInCollider(Bounds mapBounds)
    {
        for (int i = 0; i < 200; i++)
        {
            float xComponent = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float yComponent = Random.Range(mapBounds.min.y, mapBounds.max.y);
            Vector2 convertPosition = new Vector2(xComponent, yComponent);

            Collider2D[] collider = Physics2D.OverlapCircleAll(convertPosition, _levelSpawnData.MinRangeBetweenObjects);
            if (collider.Any(c => c.TryGetComponent(out CollectablePresenter _)))
                continue;

            return convertPosition;
        }

        return Vector2.zero;
    }

    private async Task<Sprite> LoadSprite(CollectableType type)
    {
        if (CollectableTypeToSprite.TryGetValue(type, out var loadSprite))
            return loadSprite;
            
        if (!Enum.IsDefined(typeof(CollectableType), type))
            throw new Exception($"Requested type does not defined in Enum {typeof(CollectableType)}");
        
        string loadId = string.Format($"ItemSheet[{type.ToString().ToLower()}]");
        if (type == CollectableType.Star)
            loadId = "Star";
        
        Sprite sprite = await _spriteProvider.Load(loadId);
        
        CollectableTypeToSprite.TryAdd(type, sprite);
        return sprite;
    }
}