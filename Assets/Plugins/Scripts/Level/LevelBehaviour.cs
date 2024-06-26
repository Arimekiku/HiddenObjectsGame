using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField] private SpawnData _data;
    [SerializeField] private BoxCollider2D _mapBounds;
    
    private TapInput _tapInput;
    private LevelSpawner _levelSpawner;
    private List<IClickable> _clickables;
    
    [Inject]
    private void Construct(MonoFactory factory, TapInput tapInput)
    {
        _levelSpawner = new LevelSpawner(factory, _data);

        _tapInput = tapInput;
    }

    private void Awake()
    {
        _clickables = new List<IClickable>();
        
        for (int i = 0; i < _data.SpawnNumber; i++)
        {
            HiddenObject newHiddenObject = _levelSpawner.SpawnAndPlaceEntity<HiddenObject>(_mapBounds);
            _clickables.Add(newHiddenObject);
        }
    }

    private void OnEnable()
    {
        _tapInput.OnTouchPerformedEvent += OnEntityClicked;
    }

    private void OnDisable()
    {
        _tapInput.OnTouchPerformedEvent -= OnEntityClicked;
    }

    private void OnDestroy()
    {
        _clickables.Clear();
    }

    private void OnEntityClicked(IClickable entity)
    {
        IClickable clickable = _clickables.First(c => c == entity);
        clickable.Click();
        
        Debug.Log($"Clicked on {clickable}");
    }
}