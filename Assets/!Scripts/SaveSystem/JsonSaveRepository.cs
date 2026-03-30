using UnityEngine;

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