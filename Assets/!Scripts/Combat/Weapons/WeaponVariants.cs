using UnityEngine;

public class SwordWeapon : MeleeWeapon
{
    public override void DealDamage()
    {
        // Simple sword logic, could add bleed effect
        base.DealDamage();
    }
}

public class AxeWeapon : MeleeWeapon
{
    public override void DealDamage()
    {
        // Axe does more damage but maybe slower
        _dmg = Mathf.RoundToInt(_dmg * 1.5f);
        base.DealDamage();
    }
}

public class WandWeapon : RangeWeapon
{
    public override void Shoot()
    {
        // Wand could shoot multiple or homing projectiles
        base.Shoot();
    }
}

public class BowWeapon : RangeWeapon
{
    public override void Shoot()
    {
        // Bow could pierce or have higher velocity
        base.Shoot();
    }
}
