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

public class JsonSaveService : ISaveService
{
    private const string SaveKey = "SaveData";

    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Игра сохранена: " + json);
    }

    public SaveData LoadGame()
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

public class UnitySceneLoader : ISceneLoaderService
{
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f; // Сбрасываем паузу при загрузке
        SceneManager.LoadScene(buildIndex);
    }
}