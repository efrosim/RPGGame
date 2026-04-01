using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerSaveHandler : MonoBehaviour, ISaveable
{
    private PlayerController _player;
    private SaveableRegistry _registry;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
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
        data.playerPosX = _player.transform.position.x;
        data.playerPosY = _player.transform.position.y;
        data.playerPosZ = _player.transform.position.z;
        data.playerHealth = _player.HP;
    }

    public void LoadState(SaveData data)
    {
        _player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
        _player._rb.linearVelocity = Vector3.zero;
        _player.SetHealth(data.playerHealth);
    }
}