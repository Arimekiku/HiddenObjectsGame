using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : ILevelSpawner
{
    private readonly LevelSpawnData _levelSpawnData;
    private readonly CollectableFactory _collectableFactory;
    private readonly ProducerFactory _producerFactory;
    private readonly ISpriteProvider _spriteProvider;

    private readonly Dictionary<CollectableType, Sprite> CollectableTypeToSprite;

    private LevelSpawner(LevelSpawnData levelSpawnData, CollectableFactory collectableFactory, ISpriteProvider spriteProvider, ProducerFactory producerFactory)
    {
        _levelSpawnData = levelSpawnData;
        _collectableFactory = collectableFactory;
        _producerFactory = producerFactory;
        _spriteProvider = spriteProvider;

        CollectableTypeToSprite = new Dictionary<CollectableType, Sprite> { { CollectableType.Empty, null } };
    }

    public CollectablePresenter SpawnAndPlaceCollectable(uint radius, Transform center, CollectableType type)
    {
        CollectablePresenter instance = _collectableFactory.Create();
        instance.Initialize(type);

        Vector2 randomPoint = GetRandomPointInCircle(radius, center);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(0.5f, _levelSpawnData.MaxInstanceScale);
        instance.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        instance.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        
        LoadSprite(instance, type);
        return instance;
    }

    public CollectablePresenter SpawnAndPlaceCollectable(HiddenObjectSaveData saveData)
    {
        CollectablePresenter instance = _collectableFactory.Create();
        instance.Initialize(saveData.Type);
        
        instance.transform.position = saveData.Position;
        instance.transform.localScale = saveData.Scale;
        instance.transform.rotation = Quaternion.Euler(saveData.Rotation);
        
        LoadSprite(instance, saveData.Type);
        return instance;
    }

    public ProducerPresenter SpawnAndPlaceProducer(uint radius, Transform center, bool isCollected)
    {
        ProducerPresenter instance = _producerFactory.Create();
        instance.Model.Initialize(isCollected);
        
        Vector2 randomPoint = GetRandomPointInCircle(radius, center);
        instance.transform.position = randomPoint;

        LoadSprite(instance);
        return instance;
    }
    
    public ProducerPresenter SpawnAndPlaceProducer(ProducerSaveData saveData)
    {
        ProducerPresenter instance = _producerFactory.Create();
        instance.Id = saveData.Id;
        instance.Model.Initialize(saveData.IsCollected);
        
        instance.transform.position = saveData.Position;

        LoadSprite(instance);
        return instance;
    }

    private async void LoadSprite(CollectablePresenter instance, CollectableType type)
    {
        Sprite sprite = await LoadSprite(type);
        instance.Model.UpdateSprite(sprite);
    }

    private async void LoadSprite(ProducerPresenter instance)
    {
        Sprite sprite = await LoadSprite(CollectableType.Producer);
        instance.Model.UpdateSprite(sprite);
    } 
    
    private Vector2 GetRandomPointInCircle(float radius, Transform center)
    {
        float randomMultiplier = Random.Range(0, radius);
        Vector2 randomPointOnCircle = Random.insideUnitCircle;

        Vector3 resultPosition = randomPointOnCircle * randomMultiplier;

        return resultPosition + center.position;
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