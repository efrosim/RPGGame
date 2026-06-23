public class BurnEffect : StatusEffect
{
    public int DamagePerSecond { get; }
    private float _tickTimer;

    public BurnEffect(int damagePerSecond, float duration) : base(duration)
    {
        DamagePerSecond = damagePerSecond;
        _tickTimer = 0f;
    }

    protected override void OnTick(PlayerModel model, float deltaTime)
    {
        _tickTimer += deltaTime;
        if (_tickTimer >= 1.0f)
        {
            _tickTimer -= 1.0f;
            if (model.Health > 0)
            {
                model.Health -= DamagePerSecond;
            }
        }
    }
}
