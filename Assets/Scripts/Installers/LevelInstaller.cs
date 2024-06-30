using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private LevelSpawnData _levelSpawnData;
    [SerializeField] private LevelCurrencyData _currencyData;
    [SerializeField] private ProducerData _producerData;
    [SerializeField] private AbilitiesData _abilitiesData;

    [Header("Presenters")] 
    [SerializeField] private CollectablePresenter _collectablePresenterPrefab;
    [SerializeField] private CollectableUIPresenter _uiPresenterPrefab;
    [SerializeField] private ProducerPresenter _producerPresenterPrefab;
    [SerializeField] private CounterProvider _counterContainer;
    
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
        Container.BindInstance(_producerData);
        Container.BindInstance(_abilitiesData);
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<CameraTracker>().AsSingle();
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