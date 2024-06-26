using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindFactories();
        BindTapSystem();
    }

    private void BindFactories()
    {
        Container
            .Bind<MonoFactory>()
            .FromNew()
            .AsSingle();
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
}