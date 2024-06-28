using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [FormerlySerializedAs("_spawnData")]
    [Header("Configs")]
    [SerializeField] private LevelSpawnData _levelSpawnData;

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
        Container.BindInstance(_levelSpawnData);
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<TapHandler>().AsSingle().WithArguments(_cameraTracker);
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