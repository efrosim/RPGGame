using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySaveHandler : MonoBehaviour, ISaveable
{
    private Enemy _enemy;
    private SaveableRegistry _registry;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        // Получаем реестр и записываемся в него
        _registry = ServiceLocator.Get<SaveableRegistry>();
        _registry.Register(this);
    }

    private void OnDisable()
    {
        // Выписываемся из реестра
        if (_registry != null)
            _registry.Unregister(this);
    }

    // Реализация интерфейса ISaveable
    public void SaveState(SaveData data)
    {
        data.enemies.Add(new EnemySaveData
        {
            id = _enemy.UniqueId,
            posX = _enemy.transform.position.x,
            posY = _enemy.transform.position.y,
            posZ = _enemy.transform.position.z,
            hp = _enemy.HP
        });
    }

    public void LoadState(SaveData data)
    {
        var savedEnemy = data.enemies.Find(e => e.id == _enemy.UniqueId);
        if (savedEnemy != null)
        {
            if (_enemy.Agent != null)
                _enemy.Agent.Warp(new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ));
            else
                _enemy.transform.position = new Vector3(savedEnemy.posX, savedEnemy.posY, savedEnemy.posZ);

            _enemy.SetHealth(savedEnemy.hp);
        }
        else
        {
            // Если врага нет в сохранении (убит до сейва), уничтожаем его
            Destroy(gameObject);
        }
    }
}