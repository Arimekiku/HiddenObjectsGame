using Zenject;

public class HiddenObjectInstaller : MonoInstaller<HiddenObjectInstaller>
{
    [Inject] private readonly HiddenObjectData _data;
    
    public override void InstallBindings()
    {
        Container.Bind<HiddenObjectData>().FromInstance(_data).AsSingle().WhenInjectedInto<HiddenObject>();
        Container.BindInterfacesAndSelfTo<HiddenObject>().FromComponentsInHierarchy().AsSingle();
    }
}