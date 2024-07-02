using Zenject;

public class CounterInstaller : MonoInstaller<CounterInstaller>
{
    [Inject] private int _id;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Counter>().AsSingle();
        Container.Bind<int>().FromInstance(_id).AsSingle().WhenInjectedInto<CollectableUICounter>();
        Container.BindInterfacesAndSelfTo<CollectableUICounter>().FromComponentsInHierarchy().AsSingle();
    }
}