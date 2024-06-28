using UnityEngine;
using Zenject;

public class LevelPresenter : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _mapBounds;

    [Inject] private LevelModel _model;

    private void Awake()
    {
        _model.SpawnEntitiesInBounds(_mapBounds.bounds);
    }
}