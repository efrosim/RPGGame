using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityAudioService : IAudioService
{
    private const string VolumeKey = "GameVolume";

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() => PlayerPrefs.GetFloat(VolumeKey, 1f);
}

public class UnitySceneLoader : ISceneLoaderService
{
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(buildIndex);
    }
}

public class JsonSaveRepository : ISaveRepository
{
    private const string SaveKey = "GameSaveData";

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Данные сохранены в репозиторий: " + json);
    }

    public SaveData Load()
    {
        if (HasSave())
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public bool HasSave() => PlayerPrefs.HasKey(SaveKey);
}