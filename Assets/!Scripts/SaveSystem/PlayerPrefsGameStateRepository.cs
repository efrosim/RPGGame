using UnityEngine;

public class PlayerPrefsGameStateRepository : IGameStateRepository
{
    private const string SaveKey = "GameSaveData";

    public void Save(GameState data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Данные сохранены в репозиторий: " + json);
    }

    public GameState Load()
    {
        if (HasSave())
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameState>(json);
        }
        return null;
    }

    public bool HasSave() => PlayerPrefs.HasKey(SaveKey);
}
