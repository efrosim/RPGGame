using System;
using UnityEngine;

public class HealthModel
{
    public int HP { get; private set; }
    public int MaxHP { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnDeadEvent;

    public HealthModel(int maxHp)
    {
        MaxHP = maxHp;
        HP = maxHp;
    }

    public void TakeDamage(int damage)
    {
        if (HP <= 0) return;

        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        OnHealthChanged?.Invoke(GetHealthNormalized());

        if (HP <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    public void SetHealth(int hp)
    {
        HP = Mathf.Clamp(hp, 0, MaxHP);
        OnHealthChanged?.Invoke(GetHealthNormalized());

        if (HP <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    public float GetHealthNormalized() => MaxHP == 0 ? 0f : (float)HP / MaxHP;
}