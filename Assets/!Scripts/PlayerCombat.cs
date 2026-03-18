using System;
using UnityEngine;

// SRP: Класс отвечает только за боевку и кулдауны
public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    public int _dmg = 10;

    [Header("Range Attack & Cooldown")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;
    public float _magicCooldown = 2f;
    
    private float _lastMagicTime = -10f;
    private bool _isCooldownActive = false;
    
    public event Action<float> OnMagicCooldownChanged;

    [Header("Melee Attack")]
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

    // Оптимизация: избегаем аллокации памяти (GC) при ударах
    private Collider[] _hitColliders = new Collider[10];

    private void Update()
    {
        // Оптимизация UI: обновляем только когда идет кулдаун
        if (_isCooldownActive)
        {
            float progress = (Time.time - _lastMagicTime) / _magicCooldown;
            if (progress >= 1f)
            {
                progress = 1f;
                _isCooldownActive = false;
            }
            OnMagicCooldownChanged?.Invoke(1f - progress);
        }
    }

    public bool CanUseMagic() => !_isCooldownActive;

    public void StartMagicCooldown()
    {
        _lastMagicTime = Time.time;
        _isCooldownActive = true;
        OnMagicCooldownChanged?.Invoke(1f);
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }

    public void MeleeDamageCheck()
    {
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        
        // Используем NonAlloc версию
        int count = Physics.OverlapBoxNonAlloc(hitCenter, _hitCube / 2, _hitColliders, transform.rotation);
        
        for (int i = 0; i < count; i++)
        {
            if (_hitColliders[i].gameObject == this.gameObject) continue;
            
            if (_hitColliders[i].TryGetComponent(out IHittable target))
            {
                target.GetHit(_dmg, DamageType.Melee);
            }
        }
    }
}