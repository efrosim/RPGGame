using UnityEngine;
using System.Collections.Generic;

public class SaveGameState : ISaveInteractor
{
    private readonly IRepository<GameStateSaveData> _repository;

    public SaveGameState(IRepository<GameStateSaveData> repository)
    {
        _repository = repository;
    }

    public bool HasSave() => _repository.HasSave();

    public void SaveGame()
    {
        PlayerController player = Object.FindAnyObjectByType<PlayerController>();
        GameStateSaveData data = new GameStateSaveData
        {
            playerPosX = player.transform.position.x,
            playerPosY = player.transform.position.y,
            playerPosZ = player.transform.position.z,
            playerHealth = player.HP,
            enemies = new List<EnemySaveData>()
        };

        Enemy[] enemiesInScene = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemiesInScene)
        {
            data.enemies.Add(new EnemySaveData
            {
                id = enemy.UniqueId,
                posX = enemy.transform.position.x,
                posY = enemy.transform.position.y,
                posZ = enemy.transform.position.z,
                hp = enemy.HP
            });
        }

        _repository.Save(data);
    }

    public void LoadGame()
    {
        GameStateSaveData data = _repository.Load();
        if (data == null) return;

        PlayerController player = Object.FindAnyObjectByType<PlayerController>();
        player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
        player._rb.linearVelocity = Vector3.zero;
        player.SetHealth(data.playerHealth);

        Enemy[] enemiesInScene = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemiesInScene)
        {
            var savedEnemy = data.enemies.Find(e => e.id == enemy.UniqueId);
            if (savedEnemy != null)
            {
                if (enemy.Agent != null)
                    enemy.Agent.Warp(new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ));
                else
                    enemy.transform.position = new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ);

                enemy.SetHealth(savedEnemy.hp);
            }
            else
            {
                Object.Destroy(enemy.gameObject);
            }
        }
    }
}