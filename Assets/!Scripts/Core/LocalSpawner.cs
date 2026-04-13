using UnityEngine;
using UnityEngine.AI;
using VContainer;

public class LocalSpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemyPrefabs;
    [SerializeField] private float _spawnRadius = 5f;[SerializeField] private int _enemiesToSpawn = 3;
    [SerializeField] private bool _spawnOnStart = true;

    private IWeaponFactory _weaponFactory;

    [Inject]
    public void Construct(IWeaponFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
    }

    private void Start()
    {
        if (_spawnOnStart) SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (_enemyPrefabs == null || _enemyPrefabs.Length == 0) return;

        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Enemy prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
            Vector3 randomPos = transform.position + Random.insideUnitSphere * _spawnRadius;
            randomPos.y = transform.position.y; 

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _spawnRadius, NavMesh.AllAreas))
            {
                Enemy spawned = Instantiate(prefab, hit.position, Quaternion.identity);
                
                if (_weaponFactory != null)
                {
                    IWeapon weapon = _weaponFactory.EquipRandomWeapon(spawned);
                    spawned.InitWeapon(weapon);
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}