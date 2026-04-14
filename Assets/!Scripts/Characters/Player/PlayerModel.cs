using System;

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
}
