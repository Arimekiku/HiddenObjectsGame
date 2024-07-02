using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private LevelCurrencyData _currencyData;
    [SerializeField] private ProducerData _producerData;
    [SerializeField] private AbilitiesData _abilitiesData;

    [Header("Presenters")] 
    [SerializeField] private CollectablePresenter _collectablePresenterPrefab;
    [SerializeField] private CollectableUIPresenter _uiPresenterPrefab;
    [SerializeField] private ProducerPresenter _producerPresenterPrefab;
    [SerializeField] private CounterPresenter _uiCounterPresenterPrefab;
    
    [Header("Providers")] 
    [SerializeField] private SpriteProvider _spriteProvider;
    [SerializeField] private CounterProvider _counterContainer;
    [SerializeField] private CurrencyProvider _currencyProvider;
    
    public override void InstallBindings()
    {
        BindSaveSystem();
        BindConfig();
        BindProviders();
        BindServices();
        BindPresenters();
        BindLevel();
        BindHiddenObjects();
    }

    private void BindSaveSystem()
    {
        Container
            .BindInterfacesAndSelfTo<SaveProvider>()
            .FromNew()
            .AsSingle()
            .WithArguments(new JsonSaveMaker());
    }

    private void BindConfig()
    {
        Container.BindInstance(_currencyData);
        Container.BindInstance(_producerData);
        Container.BindInstance(_abilitiesData);
    }

    private void BindProviders()
    {
        Container.BindInterfacesAndSelfTo<SpriteProvider>().FromInstance(_spriteProvider).AsSingle();
        Container.BindInterfacesAndSelfTo<CurrencyProvider>().FromInstance(_currencyProvider).AsSingle();
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<LevelCurrencyHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraTracker>().AsSingle();
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
        Container.BindInterfacesAndSelfTo<ProducerModel>().AsTransient();
        Container.BindInterfacesAndSelfTo<CounterModel>().AsTransient();
        
        Container
            .BindFactory<CollectablePresenter, CollectableFactory>()
            .FromComponentInNewPrefab(_collectablePresenterPrefab);
        
        Container.BindFactory<CollectablePresenter, CollectableUIPresenter, CollectableUIFactory>()
            .FromComponentInNewPrefab(_uiPresenterPrefab);

        Container.BindFactory<CounterPresenter, CollectableCounterFactory>()
            .FromComponentInNewPrefab(_uiCounterPresenterPrefab);

        Container.BindFactory<ProducerPresenter, ProducerFactory>()
            .FromComponentInNewPrefab(_producerPresenterPrefab);
    }
}