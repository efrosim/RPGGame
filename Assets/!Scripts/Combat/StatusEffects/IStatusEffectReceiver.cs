public interface IStatusEffectReceiver
{
    void ApplySlow(float modifier, float duration);
    void ApplyBurn(int damagePerSecond, float duration);
}
