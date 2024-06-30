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

    public readonly ReactiveCommand<CollectablePresenter> OnCollectableClicked;

    private readonly CollectableType[] _hiddenObjectTypes;
    private readonly List<CollectablePresenter> _hiddenObjects;

    private Transform levelCenter;
    
    public LevelModel()
    {
        OnCollectableClicked = new ReactiveCommand<CollectablePresenter>();

        _hiddenObjects = new List<CollectablePresenter>();
        
        _hiddenObjectTypes = new CollectableType[]
        {
            CollectableType.Hammer,
            CollectableType.Steerwheel,
            CollectableType.Salt,
            CollectableType.Joystick,
            CollectableType.Kettle,
        };
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
                _hiddenObjects.Add(CreateNextInstance(entityData));

            foreach (ProducerSaveData producerData in _saveProvider.SaveData.ProducersData)
            {
                ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(producerData);
                producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
            }
            
            return;
        }

        for (int i = 0; i < _data.ObjectProducersNumber; i++)
        {
            ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(_data.MaxSpawnRadius, levelCenter, false);
            producer.Id = i;
            SaveProducer(producer);

            producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
        }
        
        for (int i = 0; i < _data.InitialSpawnNumber; i++)
            _hiddenObjects.Add(CreateNextInstance(_hiddenObjectTypes));
    }

    private void SpawnCoinsAndStars()
    {
        for (int i = 0; i < _data.InitialSpawnNumber; i++)
        {
            CreateNextInstance(CollectableType.Coin);

            CreateNextInstance(CollectableType.Star);
        }
    }

    private CollectablePresenter CreateNextInstance(params CollectableType[] types)
    {
        CollectableType selectedType = types[Random.Range(0, types.Length)];
        CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(_data.MaxSpawnRadius, levelCenter, selectedType);
        
        if (!types.Contains(CollectableType.Star) && !types.Contains(CollectableType.Coin))
            SaveEntity(instance);
        
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(instance);
        }).AddTo(this);

        return instance;
    }

    private CollectablePresenter CreateNextInstance(HiddenObjectSaveData hiddenObjectData)
    {
        CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(hiddenObjectData);
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(instance);
        }).AddTo(this);

        return instance;
    }

    private void OnCollectableCollect(CollectablePresenter collectable)
    {
        OnCollectableClicked.Execute(collectable);

        if (_hiddenObjects.Contains(collectable))
        {
            _hiddenObjects.Remove(collectable);
            
            if (_hiddenObjects.IsEmpty())
                _levelSwapper.LoadNextLevel();
        }
            
        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ =>
        {
            if (collectable.Model.Type == CollectableType.Star)
            {
                CreateNextInstance(CollectableType.Star);
                return;
            }

            if (collectable.Model.Type == CollectableType.Coin)
            {
                CreateNextInstance(CollectableType.Coin);
                return;
            }
            
            _hiddenObjects.Add(CreateNextInstance(_hiddenObjectTypes));
        });
    }

    private void OnProducerCollect(ProducerPresenter producer)
    {
        _saveProvider.SaveData.UpdateProducer(producer);
        _saveProvider.Save();
        
        CollectableType selectedType = _hiddenObjectTypes[Random.Range(0, _hiddenObjectTypes.Length)];
        CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(_producerData.SpawnRadius, producer.transform, selectedType);
        
        instance.Model.OnCollect.Subscribe(_ =>
        {
            OnCollectableCollect(instance);
        }).AddTo(this);
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
        }).AddTo(this);
    }

    private void SaveProducer(ProducerPresenter producer)
    {
        ProducerSaveData producerSaveData =
            new ProducerSaveData(producer.transform.localToWorldMatrix, producer.Model.IsCollected.Value, producer.Id);
        _saveProvider.SaveData.ProducersData.Add(producerSaveData);
        _saveProvider.Save();
    }
}