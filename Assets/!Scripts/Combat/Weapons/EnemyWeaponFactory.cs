using UnityEngine;

public class EnemyWeaponFactory : IWeaponFactory
{
    public IWeapon EquipRandomWeapon(Enemy enemy)
    {
        IWeapon newWeapon = null;

        if (enemy is EnemyMelee)
        {
            if (Random.value > 0.5f)
                newWeapon = enemy.gameObject.AddComponent<SwordWeapon>();
            else
                newWeapon = enemy.gameObject.AddComponent<AxeWeapon>();
        }
        else if (enemy is EnemyRange)
        {
            if (Random.value > 0.5f)
                newWeapon = enemy.gameObject.AddComponent<WandWeapon>();
            else
                newWeapon = enemy.gameObject.AddComponent<BowWeapon>();
        }

        return newWeapon;
    }
}