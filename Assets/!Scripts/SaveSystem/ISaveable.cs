public interface ISaveable
{
    void SaveState(SaveData data);
    void LoadState(SaveData data);
}