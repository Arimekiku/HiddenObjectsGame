using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelModel : DisposableEntity
{
    [Inject] private ProducerData _producerData;
    [Inject] private ILevelSpawner _levelSpawner;
    [Inject] private SaveProvider _saveProvider;
    [Inject] private SpriteProvider _spriteProvider;
    [Inject] private CameraTracker _cameraTracker;
    [Inject] private LevelCurrencyHandler _currencyHandler;

    public ReactiveCommand<CollectablePresenter> OnCollectableClicked;

    public bool IsCompleted => _hiddenObjects.IsEmpty() || _hiddenObjects.All(h => h.gameObject.activeSelf == false);
    public IReadOnlyList<CollectablePresenter> HiddenObjects => _hiddenObjects;
    public IReadOnlyList<ProducerPresenter> Producers => _producers;

    private readonly List<CollectablePresenter> _hiddenObjects;
    private readonly List<ProducerPresenter> _producers;
    private readonly List<IDisposable> _timers;

    private Transform levelCenter;

    public LevelModel()
    {
        _hiddenObjects = new List<CollectablePresenter>();

        _producers = new List<ProducerPresenter>();

        _timers = new List<IDisposable>();
    }

    public void SetupLevel(Transform center, LevelData data)
    {
        OnCollectableClicked = new ReactiveCommand<CollectablePresenter>();
        _currencyHandler.OnCurrencyCollected.Subscribe(x => OnCollectableClicked.Execute(x)).AddTo(this);
        _cameraTracker.MainCamera.backgroundColor = data.CameraColor;

        levelCenter = center;

        SpawnHiddenObjects(data);
        SpawnCoinsAndStars(data);
    }

    public void ClearLevel()
    {
        Dispose();
        
        _currencyHandler.Clear();

        foreach (var collectable in _hiddenObjects)
            Object.Destroy(collectable.gameObject);
        _hiddenObjects.Clear();

        foreach (var producer in _producers)
            Object.Destroy(producer.gameObject);
        _producers.Clear();

        _saveProvider.SaveData.ClearData();
    }

    private void SpawnHiddenObjects(LevelData data)
    {
        if (TrySpawnFromSaveFile() == true)
            return;

        SpawnFromData(data);
    }

    private bool TrySpawnFromSaveFile()
    {
        if (_saveProvider.SaveData.EntitiesData.IsEmpty())
            return false;

        if (_saveProvider.SaveData.EntitiesData.All(s => s.IsEnabled == false))
            return false;

        foreach (HiddenObjectSaveData entityData in _saveProvider.SaveData.EntitiesData)
        {
            CollectablePresenter instance = _levelSpawner.SpawnAndPlaceCollectable(entityData);
            _hiddenObjects.Add(instance);

            instance.Model.OnCollect.Subscribe(_ => OnCollectableCollect(instance)).AddTo(this);
            instance.Model.IsEnabled.Subscribe(x =>
            {
                HiddenObjectSaveData saveData =
                    _saveProvider.SaveData.EntitiesData.First(e => e.UniqueId == instance.UniqueId);
                saveData.IsEnabled = x;
            }).AddTo(this);
        }

        foreach (ProducerSaveData producerData in _saveProvider.SaveData.ProducersData)
        {
            ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(producerData);
            _producers.Add(producer);

            producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
        }

        return true;
    }

    private void SpawnFromData(LevelData data)
    {
        for (int i = 0; i < data.MaxSpawnNumber; i++)
        {
            Sprite sprite = _spriteProvider.GetRandomSprite();

            CollectablePresenter instance =
                _levelSpawner.SpawnAndPlaceCollectable(data.MaxSpawnRadius, levelCenter, sprite);

            instance.Model.UpdateVisibility(false);
            instance.UniqueId = i;

            _hiddenObjects.Add(instance);
            SaveEntity(instance);

            instance.Model.OnCollect.Subscribe(_ => OnCollectableCollect(instance)).AddTo(this);
            instance.Model.IsEnabled.Subscribe(x =>
            {
                HiddenObjectSaveData saveData =
                    _saveProvider.SaveData.EntitiesData.First(e => e.UniqueId == instance.UniqueId);
                saveData.IsEnabled = x;
            }).AddTo(this);
        }

        for (int i = 0; i < data.ObjectProducersNumber; i++)
        {
            ProducerPresenter producer = _levelSpawner.SpawnAndPlaceProducer(data.MaxSpawnRadius, levelCenter);

            int spriteCode = _spriteProvider.GetRandomSprite().GetHashCode();
            producer.Initialize(spriteCode, i);

            _producers.Add(producer);
            SaveProducer(producer);

            producer.Model.OnCollect.Subscribe(_ => OnProducerCollect(producer)).AddTo(this);
        }

        for (int i = 0; i < data.InitialSpawnNumber; i++)
            EnableFirstValidObject();
    }

    private void SpawnCoinsAndStars(LevelData data)
    {
        for (int i = 0; i < data.InitialSpawnNumber; i++)
        {
            _currencyHandler.SpawnCoin(data, levelCenter);
            
            _currencyHandler.SpawnStar(data, levelCenter);
        }
    }

    private void EnableFirstValidObject()
    {
        for (int i = 0; i < 100; i++)
        {
            int index = Random.Range(0, _hiddenObjects.Count);
            CollectablePresenter instance = _hiddenObjects[index];

            if (instance.gameObject.activeSelf != false)
                continue;

            instance.Model.UpdateVisibility(true);
            break;
        }
    }

    private void OnCollectableCollect(CollectablePresenter collectable)
    {
        _hiddenObjects.Remove(collectable);

        Observable.Timer(TimeSpan.FromSeconds(10f)).Subscribe(_ => { EnableFirstValidObject(); }).AddTo(this);
        
        OnCollectableClicked.Execute(collectable);
    }

    private void OnProducerCollect(ProducerPresenter producer)
    {
        _saveProvider.SaveData.UpdateProducer(producer);

        var instanceSprite = _spriteProvider.GetConcreteSprite(producer.CreateId);
        var instance =
            _levelSpawner.SpawnAndPlaceCollectable(_producerData.SpawnRadius, producer.transform, instanceSprite);
        instance.Model.UpdateVisibility(true);
        
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

        entity.Model.OnCollect.Subscribe(_ =>
        {
            _saveProvider.SaveData.EntitiesData.Remove(hiddenObjectData);
            _saveProvider.Save();
        }).AddTo(this);
    }

    private void SaveProducer(ProducerPresenter producer)
    {
        _saveProvider.SaveData.AddProducer(producer);
    }
}