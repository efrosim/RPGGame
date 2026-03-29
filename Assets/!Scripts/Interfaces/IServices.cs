public interface IAudioService
{
    void SetVolume(float volume);
    float GetVolume();
}

public interface ISaveService
{
    void SaveGame(SaveData data);
    SaveData LoadGame();
    bool HasSave();
}

public interface ISceneLoaderService
{
    void LoadScene(int buildIndex);
}

// Структура данных для сохранения
[System.Serializable]
public class SaveData
{
    public float playerPosX, playerPosY, playerPosZ;
    public int playerHealth;
}