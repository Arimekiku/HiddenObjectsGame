public interface ISaveMaker
{
    public void Save<T>(T toSave, string path);
    public T Load<T>(string path);
}