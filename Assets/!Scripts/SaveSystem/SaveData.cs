using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public float playerPosX, playerPosY, playerPosZ;
    public int playerHealth;
    public List<EnemySaveData> enemies = new List<EnemySaveData>();
}

// --- МОДЕЛИ ДАННЫХ ---
[System.Serializable]
public class EnemySaveData
{
    public string id;
    public float posX, posY, posZ;
    public int hp;
}

