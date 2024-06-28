using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private SpawnData _spawnData;
    
    [Header("Presenters")]
    [SerializeField] private CollectablePresenter _presenterPrefab;

    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindConfig();
        BindServices();
        BindLevel();
        BindSpawner();
    }

    private void BindConfig()
    {
        Container.BindInstance(_spawnData);
    }

    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<CameraScroller>().AsSingle().WithArguments(_cameraTracker);
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
    }

    private void BindLevel()
    {
        Container.Bind<LevelModel>().AsSingle();
    }
    
    private void BindSpawner()
    {
        Container
            .BindFactory<ICollectableModel, CollectablePresenter, CollectableFactory>()
            .FromComponentInNewPrefab(_presenterPrefab);
    }
}