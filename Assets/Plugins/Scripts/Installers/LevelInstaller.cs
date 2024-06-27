using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [Header("Configs")]
    [SerializeField] private SpawnData _spawnData;
    
    [Header("Presenters")]
    [SerializeField] private HiddenObjectPresenter _hiddenObjectPresenterPrefab;
    [SerializeField] private StarPresenter _starPresenter;
    [SerializeField] private CoinPresenter _coinPresenter;

    [Header("Camera Tracker")] 
    [SerializeField] private CameraTracker _cameraTracker;
    
    public override void InstallBindings()
    {
        BindConfig();
        BindTapSystem();
        BindLevel();
        BindSpawner();
    }

    private void BindConfig()
    {
        Container.BindInstance(_spawnData);
    }

    private void BindTapSystem()
    {
        Container
            .Bind<CameraTracker>()
            .FromInstance(_cameraTracker)
            .AsSingle();
        
        GameObject tapContainer = new GameObject("TapSystem");
        CameraScroller cameraScroller = Container.InstantiateComponent<CameraScroller>(tapContainer);
        
        Container
            .Bind<CameraScroller>()
            .FromInstance(cameraScroller)
            .AsSingle();
    }

    private void BindLevel()
    {
        Container.Bind<LevelModel>().AsSingle();
    }
    
    private void BindSpawner()
    {
        Container.Bind<CollectableModel>().AsTransient();
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container
            .BindFactory<CollectableData, HiddenObjectPresenter, HiddenObjectFactory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<HiddenObjectInstaller>(_hiddenObjectPresenterPrefab);
        
        Container
            .BindFactory<StarData, StarPresenter, StarFactory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<StarInstaller>(_starPresenter);
        
        Container
            .BindFactory<CoinData, CoinPresenter, CoinFactory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<CoinInstaller>(_coinPresenter);
    }
}