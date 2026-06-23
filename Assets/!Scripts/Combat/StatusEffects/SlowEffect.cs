public class SlowEffect : StatusEffect
{
    public float Modifier { get; }

    public SlowEffect(float modifier, float duration) : base(duration)
    {
        Modifier = modifier;
    }
}
