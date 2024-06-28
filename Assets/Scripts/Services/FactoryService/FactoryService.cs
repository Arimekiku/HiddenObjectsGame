using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class FactoryService : IFactoryService
{
    private const string COIN_PREFAB_PATH = "Prefabs/Coin";
    private const string STAR_PREFAB_PATH = "Prefabs/Star";
    private const string HIDDEN_OBJECT_PREFAB_PATH = "Prefabs/HiddenObject";

    private readonly DiContainer _container;
    private readonly List<ICollectablePresenter> _presenters;
    
    public FactoryService(DiContainer container)
    {
        _container = container;

        _presenters = new List<ICollectablePresenter>
        {
            Resources.Load<HiddenObjectPresenter>(HIDDEN_OBJECT_PREFAB_PATH),
            Resources.Load<CoinPresenter>(COIN_PREFAB_PATH),
            Resources.Load<StarPresenter>(STAR_PREFAB_PATH),
        };
    }
    
    public T Create<T>() where T : MonoBehaviour, ICollectablePresenter
    {
        T prefab = _presenters.First(p => p is T) as T;
        T instance = _container.InstantiatePrefabForComponent<T>(prefab);

        return instance;
    }
}