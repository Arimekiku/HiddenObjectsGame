using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private LevelSpawnData _levelSpawnData;

    [Header("Presenters")] 
    [SerializeField] private CollectablePresenter _collectablePresenterPrefab;
    [SerializeField] private CurrencyPresenter _currencyPresenter;
    
    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindConfig();
        BindServices();
        BindPresenters();
        BindLevel();
        BindHiddenObjects();
    }

    private void BindConfig()
    {
        Container.BindInstance(_levelSpawnData);
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<SpriteProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<TapHandler>().AsSingle().WithArguments(_cameraTracker);
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<WalletService>().AsSingle();
    }

    private void BindPresenters()
    {
        Container.BindInstance(_currencyPresenter);    
    }
    
    private void BindLevel()
    {
        Container.Bind<LevelModel>().AsSingle();
    }

    private void BindHiddenObjects()
    {
        Container.BindInterfacesAndSelfTo<CollectableModel>().AsTransient();
        Container
            .BindFactory<Sprite, CollectableType, CollectablePresenter, CollectableFactory>()
            .FromMonoPoolableMemoryPool(
                p 
                    => p.WithInitialSize((int)(_levelSpawnData.MaxSpawnNumber * 3 + _levelSpawnData.ObjectProducersNumber))
                .FromComponentInNewPrefab(_collectablePresenterPrefab)
                .UnderTransformGroup("Collectables"));
    }
}