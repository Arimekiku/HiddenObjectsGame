using UnityEngine;

public interface IFactoryService
{
    public T Create<T>() where T: MonoBehaviour, ICollectablePresenter;
}