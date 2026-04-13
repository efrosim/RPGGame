public static class BossEffectFactory
{
    public static IBossAttackEffectStrategy GetEffect(BossElement element, bool isMelee)
    {
        if (isMelee)
        {
            return element switch
            {
                BossElement.Fire => new FireMeleeEffect(),
                BossElement.Ice => new IceMeleeEffect(),
                BossElement.Earth => new EarthMeleeEffect(),
                BossElement.Aether => new AetherMeleeEffect(),
                _ => new FireMeleeEffect()
            };
        }
        else
        {
            return element switch
            {
                BossElement.Fire => new FireRangeEffect(),
                BossElement.Ice => new IceRangeEffect(),
                BossElement.Earth => new EarthRangeEffect(),
                BossElement.Aether => new AetherRangeEffect(),
                _ => new FireRangeEffect()
            };
        }
    }
}