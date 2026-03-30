using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveInteractor
{
    private readonly ISaveRepository _repository;
    private readonly PlayerController _player;

    public SaveInteractor(ISaveRepository repository, PlayerController player)
    {
        _repository = repository;
        _player = player;
    }

    public bool HasSave() => _repository.HasSave();

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            playerPosX = _player.transform.position.x,
            playerPosY = _player.transform.position.y,
            playerPosZ = _player.transform.position.z,
            playerHealth = _player.HP,
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
        SaveData data = _repository.Load();
        if (data == null) return;

        // Восстанавливаем игрока
        _player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
        _player._rb.linearVelocity = Vector3.zero;
        _player.SetHealth(data.playerHealth);

        // Восстанавливаем врагов
        Enemy[] enemiesInScene = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemiesInScene)
        {
            var savedEnemy = data.enemies.Find(e => e.id == enemy.UniqueId);
            if (savedEnemy != null)
            {
                // Телепортируем (для NavMeshAgent нужен Warp)
                if (enemy.Agent != null)
                    enemy.Agent.Warp(new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ));
                else
                    enemy.transform.position = new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ);

                enemy.SetHealth(savedEnemy.hp);
            }
            else
            {
                // Если врага нет в сохранении (он был убит до сохранения), уничтожаем его
                Object.Destroy(enemy.gameObject);
            }
        }
    }
}