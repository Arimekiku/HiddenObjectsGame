using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private LevelSpawnData _levelSpawnData;

    [Header("Presenters")] 
    [SerializeField] private CurrencyPresenter _currencyPresenter;
    
    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindConfig();
        BindServices();
        BindPresenters();
        BindLevel();
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
        Container.BindInterfacesAndSelfTo<FactoryService>().AsSingle();
    }

    private void BindPresenters()
    {
        Container.BindInstance(_currencyPresenter);    
    }
    
    private void BindLevel()
    {
        Container.BindInterfacesAndSelfTo<CollectableModel>().AsTransient();
        Container.Bind<LevelModel>().AsSingle();
    }
}