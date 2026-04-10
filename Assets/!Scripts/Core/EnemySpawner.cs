using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Boss _bossPrefab;
    [SerializeField] private float _spawnRadius = 20f;
    [SerializeField] private int _initialEnemies = 5;

    private void Start()
    {
        for (int i = 0; i < _initialEnemies; i++)
        {
            SpawnRandomEnemy();
        }

        var entryPoint = UnityEngine.Object.FindAnyObjectByType<GameplayEntryPoint>();
        if (entryPoint != null && entryPoint.GameController != null)
        {
            entryPoint.GameController.OnBossSpawnRequested += SpawnBoss;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (_enemyPrefabs.Count == 0) return;
        
        Enemy prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        Vector3 point = GetRandomPointOnNavMesh();
        
        if (point != Vector3.zero)
        {
            Instantiate(prefab, point, Quaternion.identity);
        }
    }

    private void SpawnBoss()
    {
        if (_bossPrefab == null) return;
        Vector3 point = GetRandomPointOnNavMesh();
        if (point != Vector3.zero)
        {
            Instantiate(_bossPrefab, point, Quaternion.identity);
            Debug.Log("Boss Spawned!");
        }
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _spawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, _spawnRadius, 1))
        {
            return hit.position;
        }
        return Vector3.zero; // Fallback
    }

    private void OnDestroy()
    {
        var entryPoint = UnityEngine.Object.FindAnyObjectByType<GameplayEntryPoint>();
        if (entryPoint != null && entryPoint.GameController != null)
        {
            entryPoint.GameController.OnBossSpawnRequested -= SpawnBoss;
        }
    }
}
