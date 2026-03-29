using System.Collections.Generic;

public interface IAudioService
{
    void SetVolume(float volume);
    float GetVolume();
}

public interface ISceneLoaderService
{
    void LoadScene(int buildIndex);
}

// --- REPOSITORY PATTERN ---
public interface ISaveRepository
{
    void Save(SaveData data);
    SaveData Load();
    bool HasSave();
}

// --- INTERACTOR PATTERN ---
public interface ISaveInteractor
{
    void SaveGame();
    void LoadGame();
    bool HasSave();
}

// --- МОДЕЛИ ДАННЫХ ---
[System.Serializable]
public class EnemySaveData
{
    public string id;
    public float posX, posY, posZ;
    public int hp;
}

[System.Serializable]
public class SaveData
{
    public float playerPosX, playerPosY, playerPosZ;
    public int playerHealth;
    public List<EnemySaveData> enemies = new List<EnemySaveData>();
}