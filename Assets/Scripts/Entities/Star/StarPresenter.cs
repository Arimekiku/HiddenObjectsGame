using UnityEngine;
using Zenject;

public class StarPresenter : MonoBehaviour, ICollectablePresenter
{
    [SerializeField] private StarData _data;

    [Inject] private ICollectableModel _model;
    
    public void Collect()
    {
        _model.Collect();
    }
}