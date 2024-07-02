using Zenject;

public class SaveProvider : IInitializable
{
    private readonly string _localSaveFilename;
    private readonly string _globalSaveFilename;
    private readonly ISaveMaker _saveMaker;

    public LevelSaveData SaveData;
    public CurrencySaveData CurrencyData;

    public SaveProvider(ISaveMaker saveMaker)
    {
        _saveMaker = saveMaker;
        
        _localSaveFilename = "Level.txt";
        _globalSaveFilename = "Currency.txt";
    }
    
    public void Initialize()
    {
        if (TryLoad() != false) 
            return;
        
        SaveData = new LevelSaveData(0);
        CurrencyData = new CurrencySaveData();
    }

    public void Save()
    {
        _saveMaker.Save(SaveData, _localSaveFilename);
        _saveMaker.Save(CurrencyData, _globalSaveFilename);
    }

    public bool TryLoad()
    {
        SaveData = _saveMaker.Load<LevelSaveData>(_localSaveFilename);
        CurrencyData = _saveMaker.Load<CurrencySaveData>(_globalSaveFilename);

        return SaveData != null && CurrencyData != null;
    }
}