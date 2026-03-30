using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IHittable, IHealth, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected int _MaxHP;
    
    public Animator _animator;

    // Делегируем логику в чистый C# класс
    protected HealthModel _healthModel;

    public int HP => _healthModel?.HP ?? 0;
    public int MaxHP => _MaxHP;

    public Vector3 TargetPosition => transform.position;
    public bool IsValidTarget => HP > 0;

    public event Action<float> OnHealthChanged; 
    public event Action OnDeadEvent; 
    
    protected virtual void Awake() 
    {
        _healthModel = new HealthModel(_MaxHP);
        
        // Пробрасываем события из модели наружу
        _healthModel.OnHealthChanged += (val) => OnHealthChanged?.Invoke(val);
        _healthModel.OnDeadEvent += () => OnDeadEvent?.Invoke();
    }

    protected virtual void Start()
    {
        OnHealthChanged?.Invoke(GetHealthNormalized()); 
    }

    public void GetHit(int dmg, DamageType type)
    {
        if (HP <= 0) return; 

        _healthModel.TakeDamage(dmg);
        OnHitReceived(dmg, type);
    }

    public void SetHealth(int hp)
    {
        _healthModel.SetHealth(hp);
    }

    protected virtual void OnHitReceived(int dmg, DamageType type) { }

    public float GetHealthNormalized() => _healthModel?.GetHealthNormalized() ?? 0f;
}