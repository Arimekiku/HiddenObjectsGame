using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : ILevelSpawner
{
    private const float MAX_OBJECT_SCALE = 3;
    private const float MIN_OBJECT_SCALE = 0.5f;
    
    private readonly CollectableFactory _collectableFactory;
    private readonly ProducerFactory _producerFactory;
    private readonly ISpriteProvider _spriteProvider;

    private LevelSpawner(CollectableFactory collectableFactory, ISpriteProvider spriteProvider, ProducerFactory producerFactory)
    {
        _collectableFactory = collectableFactory;
        _producerFactory = producerFactory;
        _spriteProvider = spriteProvider;
    }
    
    public CollectablePresenter SpawnAndPlaceCollectable(uint radius, Transform center, Sprite sprite)
    {
        CollectablePresenter instance = _collectableFactory.Create();
        instance.Model.UpdateSprite(sprite);

        Vector2 randomPoint = GetRandomPointInCircle(radius, center);
        instance.transform.position = randomPoint;

        float randomMultiplier = Random.Range(MIN_OBJECT_SCALE, MAX_OBJECT_SCALE);
        instance.transform.localScale *= randomMultiplier;

        float randomRotation = Random.Range(0f, 360);
        instance.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        
        return instance;
    }

    public CollectablePresenter SpawnAndPlaceCollectable(HiddenObjectSaveData saveData)
    {
        CollectablePresenter instance = _collectableFactory.Create();
        instance.Model.UpdateVisibility(saveData.IsEnabled);

        instance.UniqueId = saveData.UniqueId;

        Sprite concreteSprite = _spriteProvider.GetConcreteSprite(saveData.SpriteId);
        instance.Model.UpdateSprite(concreteSprite);
        
        instance.transform.position = saveData.Position;
        instance.transform.localScale = saveData.Scale;
        instance.transform.rotation = Quaternion.Euler(saveData.Rotation);
        
        return instance;
    }

    public ProducerPresenter SpawnAndPlaceProducer(uint radius, Transform center)
    {
        ProducerPresenter instance = _producerFactory.Create();
        instance.Model.Initialize(false);

        Sprite sprite = _spriteProvider.GetProducerSprite();
        instance.Model.UpdateSprite(sprite);
        
        Vector2 randomPoint = GetRandomPointInCircle(radius, center);
        instance.transform.position = randomPoint;

        return instance;
    }
    
    public ProducerPresenter SpawnAndPlaceProducer(ProducerSaveData saveData)
    {
        ProducerPresenter instance = _producerFactory.Create();
        
        instance.Initialize(saveData.CreateId, saveData.UniqueId);
        instance.Model.Initialize(saveData.IsCollected);
        
        Sprite sprite = _spriteProvider.GetProducerSprite();
        instance.Model.UpdateSprite(sprite);
        
        instance.transform.position = saveData.Position;

        return instance;
    }
    
    private Vector2 GetRandomPointInCircle(float radius, Transform center)
    {
        float randomMultiplier = Random.Range(0, radius);
        Vector2 randomPointOnCircle = Random.insideUnitCircle;

        Vector3 resultPosition = randomPointOnCircle * randomMultiplier;

        return resultPosition + center.position;
    }
}