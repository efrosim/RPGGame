using UnityEngine;

public class JsonGameStateRepository : IRepository<GameStateSaveData>
{
    private const string SaveKey = "GameSaveData";

    public void Save(GameStateSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Данные сохранены в репозиторий: " + json);
    }

    public GameStateSaveData Load()
    {
        if (HasSave())
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameStateSaveData>(json);
        }
        return null;
    }

    public bool HasSave() => PlayerPrefs.HasKey(SaveKey);
}