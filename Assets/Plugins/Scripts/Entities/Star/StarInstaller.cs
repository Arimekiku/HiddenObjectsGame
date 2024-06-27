using Zenject;

public class StarInstaller : MonoInstaller<StarInstaller>
{
    [Inject] private StarData _data;

    public override void InstallBindings()
    {
        Container.Bind<StarData>().FromInstance(_data).AsSingle().WhenInjectedInto<StarPresenter>();
        Container.BindInterfacesAndSelfTo<StarPresenter>().FromComponentsInHierarchy().AsSingle();
    }
}