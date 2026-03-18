using System;
using UnityEngine;

// LSP: Character реализует ITargetable, чтобы враги могли на него охотиться
public abstract class Character : MonoBehaviour, IHittable, IHealth, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MaxHP;
    
    public Animator _animator;

    public int HP => _HP;
    public int MaxHP => _MaxHP;

    // Реализация ITargetable
    public Vector3 TargetPosition => transform.position;
    public bool IsValidTarget => _HP > 0;

    public event Action<float> OnHealthChanged; 
    public event Action OnDeadEvent; 
    
    protected virtual void Awake() { }

    protected virtual void Start()
    {
        _HP = _MaxHP;
        OnHealthChanged?.Invoke(GetHealthNormalized()); 
    }

    public virtual void GetHit(int dmg, DamageType type)
    {
        // LSP: Защита от получения урона после смерти
        if (_HP <= 0) return; 

        _HP -= dmg;
        OnHealthChanged?.Invoke(GetHealthNormalized());

        if (_HP <= 0) 
        {
            OnDeadEvent?.Invoke();
        }
    }

    public float GetHealthNormalized() => _MaxHP == 0 ? 0f : (float)_HP / _MaxHP;
}