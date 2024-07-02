using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelSwapper : ILevelSwapper, IInitializable
{
    [Inject] private SaveProvider _saveProvider;
    
    private List<LevelData> _levels;
    private int _currentLevelIndex;
    private int _levelCount;

    public void Initialize()
    {
        _levels = Resources.LoadAll<LevelData>("SO/Levels").ToList();
        _levelCount = _levels.Count;

        _currentLevelIndex = _saveProvider.SaveData.LevelIndex;
    }

    public LevelData LoadNextLevel()
    {
        _currentLevelIndex = (_currentLevelIndex + 1) % _levelCount;

        _saveProvider.SaveData.LevelIndex = _currentLevelIndex;
        
        LevelData nextLevel = _levels[_currentLevelIndex];
        return nextLevel;
    }

    public LevelData LoadCurrentLevel()
    {
        return _levels[_currentLevelIndex];
    }
}