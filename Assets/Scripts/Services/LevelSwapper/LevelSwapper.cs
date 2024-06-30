using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwapper : ILevelSwapper, IInitializable
{
    private int _levelCount = 2;

    public void Initialize()
    {
        _levelCount = SceneManager.sceneCountInBuildSettings;
    }
    
    public void LoadNextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene((currentScene.buildIndex + 1) % _levelCount);
    }
}