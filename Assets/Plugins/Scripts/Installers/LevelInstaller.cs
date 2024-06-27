using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller<LevelInstaller>
{
    [SerializeField] private HiddenObject _hiddenObjectPrefab;
    [SerializeField] private LevelBehaviour _levelBehaviour;
    
    public override void InstallBindings()
    {
        BindTapSystem();
        BindLevel();
        BindFactories();
    }

    private void BindTapSystem()
    {
        GameObject tapContainer = new GameObject("TapSystem");
        
        TapInput tapInput = Container.InstantiateComponent<TapInput>(tapContainer);

        Container
            .Bind<TapInput>()
            .FromInstance(tapInput)
            .AsSingle();
    }

    private void BindLevel()
    {
        Container
            .BindInterfacesAndSelfTo<LevelBehaviour>()
            .FromInstance(_levelBehaviour);
    }
    
    private void BindFactories()
    {
        Container
            .BindFactory<HiddenObjectData, HiddenObject, HiddenObjectFactory>()
            .FromSubContainerResolve()
            .ByNewContextPrefab<HiddenObjectInstaller>(_hiddenObjectPrefab);
    }
}