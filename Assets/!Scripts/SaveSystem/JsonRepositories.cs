using UnityEngine;

public class PlayerJsonRepository : IPlayerRepository
{
    private const string SaveKey = "PlayerSaveData";

    public void Save(PlayerSaveData data)
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public PlayerSaveData Load()
    {
        if (HasSave())
        {
            return JsonUtility.FromJson<PlayerSaveData>(PlayerPrefs.GetString(SaveKey));
        }
        return null;
    }

    public bool HasSave() => PlayerPrefs.HasKey(SaveKey);
}

public class EnemyJsonRepository : IEnemyRepository
{
    private const string SaveKey = "EnemySaveData";

    public void Save(EnemySaveDataList data)
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public EnemySaveDataList Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            return JsonUtility.FromJson<EnemySaveDataList>(PlayerPrefs.GetString(SaveKey));
        }
        return new EnemySaveDataList { enemies = new System.Collections.Generic.List<EnemySaveData>() };
    }
}
