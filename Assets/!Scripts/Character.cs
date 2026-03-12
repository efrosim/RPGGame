using System;
using System.Xml.Serialization;
using UnityEditor.Animations;
using UnityEngine;

public enum DamageType
{
    Melee,
    Range
}

public abstract class Character : MonoBehaviour, IHittable
{
    [Header("Stats")]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MaxHP;
    [SerializeField] protected int _dmg;
    public float _reload;
    public float _moveSpeed;
    public float _rotSpeed;

    public Animator _animator;
    //Audio Controller?

    // Событие, которое будет вызываться при изменении здоровья
    public event Action<float> OnHealthChanged; 
    
    private void Start()
    {
        _HP = _MaxHP;
        OnHealthChanged?.Invoke(GetHealthNormalized()); 
    }

    public virtual void GetHit(int dmg)
    {
        _HP -= dmg;
        OnHealthChanged?.Invoke(GetHealthNormalized());
        //Anim of get damage
 //       if (_HP < 1) Dead();
    }

    public virtual void DealDmg()
    {
        
    }
    
    public float GetHealthNormalized()
    {
        if (_MaxHP == 0) return 0f; // Защита от деления на ноль
        return (float)_HP / _MaxHP;
    }
}
