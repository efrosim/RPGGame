using UnityEngine;

public class EnemyWeaponFactory : IWeaponFactory
{
    public IWeapon EquipRandomWeapon(Enemy enemy)
    {
        IWeapon newWeapon = null;

        if (enemy is EnemyMelee)
        {
            var oldMelee = enemy.GetComponent<MeleeWeapon>();
            
            MeleeWeapon newMeleeComponent;
            if (Random.value > 0.5f)
                newMeleeComponent = enemy.gameObject.AddComponent<SwordWeapon>();
            else
                newMeleeComponent = enemy.gameObject.AddComponent<AxeWeapon>();

            if (oldMelee != null)
            {
                newMeleeComponent._dmg = oldMelee._dmg;
                newMeleeComponent.HitCube = oldMelee.HitCube;
                newMeleeComponent.HitOffset = oldMelee.HitOffset;
                newMeleeComponent.MaxTargets = oldMelee.MaxTargets;
                
                // Уничтожаем старый дефолтный компонент оружия, чтобы избежать дублирования
                Object.Destroy(oldMelee);
            }
            newWeapon = newMeleeComponent;
        }
        else if (enemy is EnemyRange)
        {
            var oldRange = enemy.GetComponent<RangeWeapon>();
            
            RangeWeapon newRangeComponent;
            if (Random.value > 0.5f)
                newRangeComponent = enemy.gameObject.AddComponent<WandWeapon>();
            else
                newRangeComponent = enemy.gameObject.AddComponent<BowWeapon>();

            if (oldRange != null)
            {
                newRangeComponent._dmg = oldRange._dmg;
                newRangeComponent._shellPrefab = oldRange._shellPrefab;
                newRangeComponent._shellSpawnPos = oldRange._shellSpawnPos;

                // Уничтожаем старый дефолтный компонент оружия, чтобы избежать дублирования
                Object.Destroy(oldRange);
            }
            newWeapon = newRangeComponent;
        }

        return newWeapon;
    }
}