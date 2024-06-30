using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class LevelModel
{
    [Inject] private LevelSpawnData _data;
    [Inject] private ILevelSpawner _levelSpawner;
    [Inject] private SaveProvider _saveProvider;

    public readonly ReactiveCommand<CollectablePresenter> OnCollectableClicked;

    public bool IsCompleted => _hiddenObjects.IsEmpty();
    
    private readonly CollectableType[] _hiddenObjectTypes;
    private readonly List<CollectablePresenter> _hiddenObjects;
    private readonly List<ProducerPresenter> _producers;
    
    public LevelModel()
    {
        OnCollectableClicked = new ReactiveCommand<CollectablePresenter>();

        _hiddenObjects = new List<CollectablePresenter>();
        _producers = new List<ProducerPresenter>();
        
        _hiddenObjectTypes = new CollectableType[]
        {
            CollectableType.Hammer,
            CollectableType.Steerwheel,
            CollectableType.Salt,
            CollectableType.Joystick,
            CollectableType.Kettle,
        };
    }

    public void SetupLevel(Bounds mapBounds)
    {
        SpawnHiddenObjects(mapBounds);

        SpawnCoinsAndStars(mapBounds);
    }

    private void SpawnHiddenObjects(Bounds mapBounds)
    {
        if (!_saveProvider.SaveData.EntitiesData.IsEmpty())
        {
            foreach (HiddenObjectSaveData entityData in _saveProvider.SaveData.EntitiesData)
                _hiddenObjects.Add(CreateNextInstance(mapBounds, entityData));

            foreach (ProducerSaveData producerData in _saveProvider.SaveData.ProducersData)
            {
                ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(producerData);
                producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer));

                _producers.Add(producer);
            }
            
            return;
        }

        for (int i = 0; i < _data.ObjectProducersNumber; i++)
        {
            ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(mapBounds, false);
            SaveProducer(producer);

            producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer));
            _producers.Add(producer);
        }
        
        for (int i = 0; i < _data.InitialSpawnNumber; i++)
            _hiddenObjects.Add(CreateNextInstance(mapBounds, _hiddenObjectTypes));
    }

    private void SpawnCoinsAndStars(Bounds mapBounds)
    {
        for (int i = 0; i < _data.InitialSpawnNumber; i++)
        {
            CreateNextInstance(mapBounds, CollectableType.Coin);

            CreateNextInstance(mapBounds, CollectableType.Star);
        }
    }

    private CollectablePresenter CreateNextInstance(Bounds bounds, params CollectableType[] types)
    {
        CollectableType selectedType = types[Random.Range(0, types.Length)];
        CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(bounds, selectedType);
        
        if (!types.Contains(CollectableType.Star) && !types.Contains(CollectableType.Coin))
            SaveEntity(instance);
        
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(bounds, instance);
        });

        return instance;
    }

    private CollectablePresenter CreateNextInstance(Bounds bounds, HiddenObjectSaveData hiddenObjectData)
    {
        CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(hiddenObjectData);
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(bounds, instance);
        });

        return instance;
    }

    private void OnCollectableCollect(Bounds bounds, CollectablePresenter collectable)
    {
        OnCollectableClicked.Execute(collectable);
            
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ =>
        {
            if (collectable.Model.Type == CollectableType.Star)
            {
                CreateNextInstance(bounds, CollectableType.Star);
                return;
            }

            if (collectable.Model.Type == CollectableType.Coin)
            {
                CreateNextInstance(bounds, CollectableType.Coin);
                return;
            }
            
            _hiddenObjects.Add(CreateNextInstance(bounds, _hiddenObjectTypes));
        });
    }

    private void OnProducerCollect(ProducerPresenter producer)
    {
        CreateNextInstance(producer.SpawnBounds, _hiddenObjectTypes);
    }

    private void SaveEntity(CollectablePresenter entity)
    {
        HiddenObjectSaveData hiddenObjectData = new HiddenObjectSaveData(
            entity.transform.localToWorldMatrix, 
            entity.Model.Type);
        _saveProvider.SaveData.EntitiesData.Add(hiddenObjectData);
        _saveProvider.Save();
            
        entity.Model.OnCollect.Subscribe(_ =>
        {
            _saveProvider.SaveData.EntitiesData.Remove(hiddenObjectData);
            _saveProvider.Save();
        });
    }

    private void SaveProducer(ProducerPresenter producer)
    {
        ProducerSaveData producerSaveData =
            new ProducerSaveData(producer.transform.localToWorldMatrix, producer.Model.IsCollected.Value);
        _saveProvider.SaveData.ProducersData.Add(producerSaveData);
        _saveProvider.Save();
    }
}