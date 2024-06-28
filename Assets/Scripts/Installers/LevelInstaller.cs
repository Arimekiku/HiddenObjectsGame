using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private SpawnData _spawnData;

    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindConfig();
        BindServices();
        BindLevel();
    }

    private void BindConfig()
    {
        Container.BindInstance(_spawnData);
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<CameraScroller>().AsSingle().WithArguments(_cameraTracker);
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<WalletService>().AsSingle();
        Container.BindInterfacesAndSelfTo<FactoryService>().AsSingle();
    }

    private void BindLevel()
    {
        Container.BindInterfacesAndSelfTo<CollectableModel>().AsTransient();
        Container.Bind<LevelModel>().AsSingle();
    }
}