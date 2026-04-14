using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveInteractor
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IEnemyRepository _enemyRepository;

    public SaveInteractor(IPlayerRepository playerRepository, IEnemyRepository enemyRepository)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
    }

    public bool HasSave() => _playerRepository.HasSave();

    public void SaveGame()
    {
        var player = UnityEngine.Object.FindAnyObjectByType<PlayerView>();
        if (player != null)
        {
            _playerRepository.Save(new PlayerSaveData
            {
                posX = player.transform.position.x,
                posY = player.transform.position.y,
                posZ = player.transform.position.z,
                health = player.HP
            });
        }

        var enemiesInScene = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        var enemyDataList = new EnemySaveDataList { enemies = new List<EnemySaveData>() };
        foreach (var enemy in enemiesInScene)
        {
            enemyDataList.enemies.Add(new EnemySaveData
            {
                id = enemy.UniqueId,
                posX = enemy.transform.position.x,
                posY = enemy.transform.position.y,
                posZ = enemy.transform.position.z,
                hp = enemy.HP
            });
        }
        _enemyRepository.Save(enemyDataList);
    }

    public void LoadGame()
    {
        var playerData = _playerRepository.Load();
        if (playerData != null)
        {
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerView>();
            if (player != null)
            {
                player.transform.position = new Vector3(playerData.posX, playerData.posY, playerData.posZ);
                player.Rb.linearVelocity = Vector3.zero;
                if (player.Model != null)
                    player.Model.Health = playerData.health;
            }
        }

        var enemyDataList = _enemyRepository.Load();
        if (enemyDataList != null && enemyDataList.enemies != null)
        {
            var enemiesInScene = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemiesInScene)
            {
                var savedEnemy = enemyDataList.enemies.Find(e => e.id == enemy.UniqueId);
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
                    UnityEngine.Object.Destroy(enemy.gameObject);
                }
            }
        }
    }
}
