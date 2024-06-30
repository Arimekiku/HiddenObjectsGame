using UnityEngine.SceneManagement;

public class SaveProvider
{
    private readonly string _saveFilename;
    private readonly ISaveMaker _saveMaker;

    public LevelSaveData SaveData;

    public SaveProvider(ISaveMaker saveMaker)
    {
        _saveMaker = saveMaker;
        
        _saveFilename = string.Format($"Level_{SceneManager.GetActiveScene().buildIndex}.txt");
    }

    public void Save()
    {
        _saveMaker.Save(SaveData, _saveFilename);
    }

    public void Load()
    {
        SaveData = _saveMaker.Load<LevelSaveData>(_saveFilename) ?? new LevelSaveData();
    }
}