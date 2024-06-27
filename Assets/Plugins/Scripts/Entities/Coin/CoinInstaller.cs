using Zenject;

public class CoinInstaller : MonoInstaller<CoinInstaller>
{
    [Inject] private CoinData _data;

    public override void InstallBindings()
    {
        Container.Bind<CoinData>().FromInstance(_data).AsSingle().WhenInjectedInto<CoinPresenter>();
        Container.BindInterfacesAndSelfTo<CoinPresenter>().FromComponentsInHierarchy().AsSingle();
    }
}