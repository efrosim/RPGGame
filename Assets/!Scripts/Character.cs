using System;
using UnityEngine;

public enum DamageType
{
    Melee,
    Range
}

public abstract class Character : MonoBehaviour, IHittable, IHealth
{[Header("Stats")]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MaxHP;
    [SerializeField] protected int _dmg;
    public float _moveSpeed;
    public float _rotSpeed;

    public Animator _animator;

    public int HP => _HP;
    public int MaxHP => _MaxHP;

    public event Action<float> OnHealthChanged; 
    
    protected virtual void Start()
    {
        _HP = _MaxHP;
        OnHealthChanged?.Invoke(GetHealthNormalized()); 
    }

    // Принимаем тип урона
    public virtual void GetHit(int dmg, DamageType type)
    {
        _HP -= dmg;
        OnHealthChanged?.Invoke(GetHealthNormalized());
    }

    public virtual void DealDmg() { }
    
    public float GetHealthNormalized()
    {
        if (_MaxHP == 0) return 0f; 
        return (float)_HP / _MaxHP;
    }
}