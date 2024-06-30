using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CounterProvider : MonoBehaviour
{
    [SerializeField] private Canvas _parent;
    [SerializeField] private CollectableUICounter[] _holders;

    [Inject] private CollectableUIFactory _uiFactory;
    [Inject] private ICounter _counter;
    [Inject] private CameraTracker _tracker;

    public void CollectAnimation(CollectablePresenter presenter, int changeValue)
    {
        CollectableUIPresenter instance = _uiFactory.Create(presenter.Model);
        instance.transform.SetParent(_parent.transform, false);
        instance.transform.position = presenter.transform.position;

        CollectableUICounter counter = _holders.First(h => h.Counter.Type == presenter.Model.Type);
        counter.Counter.Add(changeValue);
        instance.transform
            .DOMove(counter.transform.position, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                counter.UpdateText();
                DestroyImmediate(instance.gameObject);
            });
    }
}