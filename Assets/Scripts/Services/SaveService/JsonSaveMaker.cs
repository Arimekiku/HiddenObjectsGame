using System;
using System.IO;
using UnityEngine;

public class JsonSaveMaker : ISaveMaker
{
    private readonly string _saveDataPath = Application.persistentDataPath;

    public void Save<T>(T toSave, string filename)
    {
        string dataPath = GetDataPath(filename);
        
        if (Path.GetDirectoryName(dataPath) is null)
            throw new Exception($"Invalid directory path with name {dataPath}.");

        Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
        string serializableData = JsonUtility.ToJson(toSave, true);

        using FileStream fs = new(dataPath, FileMode.Create);
        using StreamWriter sw = new(fs);
        
        sw.Write(serializableData);
    }

    public T Load<T>(string filename)
    {
        T loadData = default;
        
        string dataPath = GetDataPath(filename);
        if (File.Exists(dataPath))
        {
            using FileStream fs = new(dataPath, FileMode.Open);
            using StreamReader sr = new(fs);

            string readData = sr.ReadToEnd();

            loadData = JsonUtility.FromJson<T>(readData);
        }

        return loadData;
    }
    
    private string GetDataPath(string filename)
    {
        return Path.Combine(_saveDataPath, filename);
    }
}