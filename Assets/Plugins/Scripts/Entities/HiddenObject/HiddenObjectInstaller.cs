using Zenject;

public class HiddenObjectInstaller : MonoInstaller<HiddenObjectInstaller>
{
    [Inject] private readonly CollectableData _data;
    
    public override void InstallBindings()
    {
        Container.Bind<CollectableData>().FromInstance(_data).AsSingle().WhenInjectedInto<HiddenObjectPresenter>();
        Container.BindInterfacesAndSelfTo<HiddenObjectPresenter>().FromComponentsInHierarchy().AsSingle();
    }
}