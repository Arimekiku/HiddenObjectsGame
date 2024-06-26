using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MonoFactory
{
    private const string HIDDEN_OBJECT_PREFAB_PATH = "Prefabs/Square";
    private const string HIDDEN_OBJECT_PRODUCER_PREFAB_PATH = "";
    
    private readonly DiContainer _container;
    private readonly List<IClickable> _prefabs;
    private readonly HiddenObject _hiddenObjectPrefab;
    
    public MonoFactory(DiContainer container)
    {
        _container = container;

        _prefabs = new List<IClickable>
        {
            Resources.Load<HiddenObject>(HIDDEN_OBJECT_PREFAB_PATH),
            Resources.Load<HiddenObjectProducer>(HIDDEN_OBJECT_PRODUCER_PREFAB_PATH)
        };
    }

    public T SpawnEntity<T>() where T : MonoBehaviour, IClickable
    {
        T entityPrefab = _prefabs.First(p => p is T) as T;
        T entityInstance = _container.InstantiatePrefabForComponent<T>(entityPrefab);

        return entityInstance;
    }
}