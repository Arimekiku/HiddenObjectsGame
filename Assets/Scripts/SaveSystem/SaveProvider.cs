using UnityEngine.SceneManagement;

public class SaveProvider
{
    private readonly string _localSaveFilename;
    private readonly string _globalSaveFilename;
    private readonly ISaveMaker _saveMaker;

    public LevelSaveData SaveData;
    public CurrencySaveData CurrencyData;

    public SaveProvider(ISaveMaker saveMaker)
    {
        _saveMaker = saveMaker;
        
        _localSaveFilename = string.Format($"Level_{SceneManager.GetActiveScene().buildIndex}.txt");
        _globalSaveFilename = "Currency.txt";
    }

    public void Save()
    {
        _saveMaker.Save(SaveData, _localSaveFilename);
        _saveMaker.Save(CurrencyData, _globalSaveFilename);
    }

    public void Load()
    {
        SaveData = _saveMaker.Load<LevelSaveData>(_localSaveFilename) ?? new LevelSaveData();
        CurrencyData = _saveMaker.Load<CurrencySaveData>(_globalSaveFilename) ?? new CurrencySaveData();
    }
}