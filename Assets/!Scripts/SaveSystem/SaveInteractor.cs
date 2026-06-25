using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveInteractor
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IEnemyRepository _enemyRepository;
    private readonly IWeaponFactory _weaponFactory;

    public SaveInteractor(IPlayerRepository playerRepository, IEnemyRepository enemyRepository, IWeaponFactory weaponFactory)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
        _weaponFactory = weaponFactory;
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
            string type = "Melee";
            if (enemy is Boss) type = "Boss";
            else if (enemy is EnemyRange) type = "Range";

            enemyDataList.enemies.Add(new EnemySaveData
            {
                id = enemy.UniqueId,
                type = type,
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
            // 1. Уничтожаем всех мобов, которые сейчас есть на сцене
            var enemiesInScene = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemiesInScene)
            {
                UnityEngine.Object.Destroy(enemy.gameObject);
            }

            // 2. Находим спавнер мобов для доступа к ссылкам на префабы
            var spawner = UnityEngine.Object.FindAnyObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                foreach (var savedEnemy in enemyDataList.enemies)
                {
                    Enemy prefab = null;
                    if (savedEnemy.type == "Boss")
                    {
                        prefab = spawner.BossPrefab;
                    }
                    else if (savedEnemy.type == "Melee")
                    {
                        prefab = spawner.EnemyPrefabs.Find(p => p is EnemyMelee);
                    }
                    else if (savedEnemy.type == "Range")
                    {
                        prefab = spawner.EnemyPrefabs.Find(p => p is EnemyRange);
                    }

                    if (prefab != null)
                    {
                        Vector3 spawnPos = new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ);
                        Enemy spawned = UnityEngine.Object.Instantiate(prefab, spawnPos, Quaternion.identity);

                        spawned.SetDynamicId(savedEnemy.id);
                        spawned.SetHealth(savedEnemy.hp);

                        // Если это обычный моб (не босс) - вешаем ему оружие через фабрику
                        if (!(spawned is Boss) && _weaponFactory != null)
                        {
                            var weapon = _weaponFactory.EquipRandomWeapon(spawned);
                            spawned.InitWeapon(weapon);
                        }
                    }
                }
            }
        }
    }
}
