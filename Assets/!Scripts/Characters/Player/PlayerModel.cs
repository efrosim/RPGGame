using System;
using System.Collections.Generic;

public class PlayerModel
{
    private int _health;
    public int MaxHealth { get; private set; }
    
    public int Health
    {
        get => _health;
        set
        {
            _health = Math.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke(_health);
            if (_health <= 0) OnDead?.Invoke();
        }
    }

    public float MoveSpeed { get; private set; }
    public float RotSpeed { get; private set; }
    public CooldownTimer MagicCooldown { get; private set; }

    private readonly List<StatusEffect> _activeEffects = new List<StatusEffect>();
    public IReadOnlyList<StatusEffect> ActiveEffects => _activeEffects;

    public float SpeedModifier
    {
        get
        {
            float minModifier = 1.0f;
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                if (_activeEffects[i] is SlowEffect slow)
                {
                    if (slow.Modifier < minModifier)
                    {
                        minModifier = slow.Modifier;
                    }
                }
            }
            return minModifier;
        }
    }

    public event Action<int> OnHealthChanged;
    public event Action OnDead;

    public PlayerModel(int maxHp, float moveSpeed, float rotSpeed, float cooldownTime)
    {
        MaxHealth = maxHp;
        _health = maxHp;
        MoveSpeed = moveSpeed;
        RotSpeed = rotSpeed;
        MagicCooldown = new CooldownTimer(cooldownTime);
    }

    public void ApplyEffect(StatusEffect effect)
    {
        if (effect is SlowEffect newSlow)
        {
            var existingSlow = _activeEffects.Find(e => e is SlowEffect s && Math.Abs(s.Modifier - newSlow.Modifier) < 0.0001f) as SlowEffect;
            if (existingSlow != null)
            {
                existingSlow.ResetDuration();
                return;
            }
        }
        else if (effect is BurnEffect newBurn)
        {
            var existingBurn = _activeEffects.Find(e => e is BurnEffect b && b.DamagePerSecond == newBurn.DamagePerSecond) as BurnEffect;
            if (existingBurn != null)
            {
                existingBurn.ResetDuration();
                return;
            }
        }

        _activeEffects.Add(effect);
        effect.OnApply(this);
    }

    public void UpdateEffects(float deltaTime)
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = _activeEffects[i];
            effect.Tick(this, deltaTime);
            if (effect.IsFinished)
            {
                effect.OnRemove(this);
                _activeEffects.RemoveAt(i);
            }
        }
    }
}
