using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class LevelBehaviour : MonoBehaviour, IInitializable
{
    [SerializeField] private SpawnData _data;
    [SerializeField] private BoxCollider2D _mapBounds;
    
    private TapInput _tapInput;
    private LevelSpawner _levelSpawner;
    private List<IClickable> _clickables;
    
    [Inject]
    private void Construct(HiddenObjectFactory factory, TapInput tapInput)
    {
        _levelSpawner = new LevelSpawner(factory, _data);

        _tapInput = tapInput;
    }
    
    public void Initialize()
    {
        Observable.FromEvent<IClickable>(
                x => _tapInput.OnTouchPerformedEvent += x,
                x => _tapInput.OnTouchPerformedEvent -= x)
            .Subscribe(OnEntityClicked);
        
        _clickables = new List<IClickable>();
        
        for (int i = 0; i < _data.SpawnNumber; i++)
        {
            HiddenObject newHiddenObject = _levelSpawner.SpawnAndPlaceHiddenObject(_mapBounds);
            _clickables.Add(newHiddenObject);
        }
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