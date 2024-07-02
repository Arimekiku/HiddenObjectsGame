using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class LevelModel : DisposableEntity
{
    [Inject] private LevelSpawnData _data;
    [Inject] private ProducerData _producerData;
    [Inject] private ILevelSpawner _levelSpawner;
    [Inject] private ILevelSwapper _levelSwapper;
    [Inject] private SaveProvider _saveProvider;
    [Inject] private SpriteProvider _spriteProvider;

    public readonly ReactiveCommand<CollectablePresenter> OnCollectableClicked;

    public bool IsCompleted => _hiddenObjects.IsEmpty() || _hiddenObjects.All(h => h.gameObject.activeSelf == false);
    public IReadOnlyList<CollectablePresenter> HiddenObjects => _hiddenObjects;
    public IReadOnlyList<ProducerPresenter> Producers => _producers;

    private readonly List<CollectablePresenter> _hiddenObjects;
    private readonly List<ProducerPresenter> _producers;

    private Transform levelCenter;
    
    public LevelModel()
    {
        OnCollectableClicked = new ReactiveCommand<CollectablePresenter>();

        _hiddenObjects = new List<CollectablePresenter>();

        _producers = new List<ProducerPresenter>();
    }

    public void SetupLevel(Transform center)
    {
        levelCenter = center;
        
        SpawnHiddenObjects();

        SpawnCoinsAndStars();
    }

    private void SpawnHiddenObjects()
    {
        if (!_saveProvider.SaveData.EntitiesData.IsEmpty())
        {
            foreach (HiddenObjectSaveData entityData in _saveProvider.SaveData.EntitiesData)
            {
                CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(entityData);
                _hiddenObjects.Add(instance);
                
                instance.Model.OnCollect.Subscribe(_ => OnCollectableCollect(instance)).AddTo(this);
            }

            foreach (ProducerSaveData producerData in _saveProvider.SaveData.ProducersData)
            {
                ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(producerData);
                _producers.Add(producer);
                
                producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
            }
            
            return;
        }

        for (int i = 0; i < _data.MaxSpawnNumber; i++)
        {
            Sprite sprite = _spriteProvider.GetRandomSprite();

            CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(_data.MaxSpawnRadius, levelCenter, sprite);
            instance.gameObject.SetActive(false);
            instance.UniqueId = i;
            
            _hiddenObjects.Add(instance);
            SaveEntity(instance);

            instance.Model.OnCollect.Subscribe(_ => OnCollectableCollect(instance)).AddTo(this);
        }
        
        for (int i = 0; i < _data.ObjectProducersNumber; i++)
        {
            ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(_data.MaxSpawnRadius, levelCenter);
            
            int spriteCode = _spriteProvider.GetRandomSprite().GetHashCode();
            producer.Initialize(spriteCode, i);
            
            _producers.Add(producer);
            SaveProducer(producer);

            producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
        }
    }

    private void SpawnCoinsAndStars()
    {
        for (int i = 0; i < _data.InitialSpawnNumber; i++)
        {
            EnableFirstValidObject();
            
            //TODO: coins and stars
        }
    }
    
    private void EnableFirstValidObject()
    {
        while (true)
        {
            int index = Random.Range(0, _hiddenObjects.Count);
            CollectablePresenter instance = _hiddenObjects[index];

            if (instance.gameObject.activeSelf != false) 
                continue;
            
            _hiddenObjects[index].gameObject.SetActive(true);
            break;
        }
    }

    private void OnCollectableCollect(CollectablePresenter collectable)
    {
        _hiddenObjects.Remove(collectable);
        
        OnCollectableClicked.Execute(collectable);
        
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ =>
        {
            EnableFirstValidObject();
        }).AddTo(this);
    }

    private void OnProducerCollect(ProducerPresenter producer)
    {
        _saveProvider.SaveData.UpdateProducer(producer);

        var instanceSprite = _spriteProvider.GetConcreteSprite(producer.CreateId);
        var instance = _levelSpawner.SpawnAndPlaceCollectable(_producerData.SpawnRadius, producer.transform, instanceSprite);

        while (_hiddenObjects.Any(h => h.UniqueId == instance.UniqueId))
            instance.UniqueId += 1;
        
        _hiddenObjects.Add(instance);
        SaveEntity(instance);
        
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(instance);
            
            _saveProvider.SaveData.EntitiesData.RemoveAll(i => i.UniqueId == instance.UniqueId);
        }).AddTo(this);
    }

    private void SaveEntity(CollectablePresenter entity)
    {
        var hiddenObjectData = new HiddenObjectSaveData(entity);
        _saveProvider.SaveData.EntitiesData.Add(hiddenObjectData);
        _saveProvider.Save();
            
        entity.Model.OnCollect.Subscribe(_ =>
        {
            _saveProvider.SaveData.EntitiesData.Remove(hiddenObjectData);
            _saveProvider.Save();
        }).AddTo(this);
    }

    private void SaveProducer(ProducerPresenter producer)
    {   
        _saveProvider.SaveData.AddProducer(producer);
        _saveProvider.Save();
    }
}