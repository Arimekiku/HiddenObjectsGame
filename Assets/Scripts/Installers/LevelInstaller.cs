using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private LevelSpawnData _levelSpawnData;
    [SerializeField] private LevelCurrencyData _currencyData;

    [Header("Presenters")] 
    [SerializeField] private CollectablePresenter _collectablePresenterPrefab;
    [SerializeField] private CollectableUIPresenter _uiPresenterPrefab;
    [SerializeField] private ProducerPresenter _producerPresenterPrefab;
    [SerializeField] private CounterProvider _counterContainer;
    
    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindSaveSystem();
        BindConfig();
        BindServices();
        BindPresenters();
        BindLevel();
        BindHiddenObjects();
    }

    private void BindSaveSystem()
    {
        SaveProvider saveProvider = new SaveProvider(new JsonSaveMaker());
        saveProvider.Load();
        
        Container.BindInterfacesAndSelfTo<SaveProvider>().FromInstance(saveProvider).AsSingle();
    }

    private void BindConfig()
    {
        Container.BindInstance(_levelSpawnData);
        Container.BindInstance(_currencyData);
    }

    private void BindServices()
    {
        Container.Bind<CameraTracker>().FromInstance(_cameraTracker);
        Container.BindInterfacesAndSelfTo<SpriteProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<TapHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSwapper>().AsSingle();
    }

    private void BindPresenters()
    {
        Container.BindInstance(_counterContainer);
    }
    
    private void BindLevel()
    {
        Container.Bind<LevelModel>().AsSingle();
    }

    private void BindHiddenObjects()
    {
        Container.BindInterfacesAndSelfTo<CollectableModel>().AsTransient();
        Container
            .BindFactory<CollectablePresenter, CollectableFactory>()
            .FromComponentInNewPrefab(_collectablePresenterPrefab);
        
        Container.BindInterfacesAndSelfTo<Counter>().AsTransient();
        Container.BindFactory<CollectableModel, CollectableUIPresenter, CollectableUIFactory>()
            .FromComponentInNewPrefab(_uiPresenterPrefab);

        Container.BindInterfacesAndSelfTo<ProducerModel>().AsTransient();
        Container.BindFactory<ProducerPresenter, ProducerFactory>()
            .FromComponentInNewPrefab(_producerPresenterPrefab);
    }
}