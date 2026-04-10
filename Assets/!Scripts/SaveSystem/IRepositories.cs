public interface IPlayerRepository
{
    void Save(PlayerSaveData data);
    PlayerSaveData Load();
    bool HasSave();
}

public interface IEnemyRepository
{
    void Save(EnemySaveDataList data);
    EnemySaveDataList Load();
}

[System.Serializable]
public class PlayerSaveData
{
    public float posX, posY, posZ;
    public int health;
}

[System.Serializable]
public class EnemySaveData
{
    public string id;
    public float posX, posY, posZ;
    public int hp;
}

[System.Serializable]
public class EnemySaveDataList
{
    public System.Collections.Generic.List<EnemySaveData> enemies;
}
