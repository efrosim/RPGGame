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
                CopyFields(oldMelee, newMeleeComponent);
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

                CopyFields(oldRange, newRangeComponent);
                // Уничтожаем старый дефолтный компонент оружия, чтобы избежать дублирования
                Object.Destroy(oldRange);
            }
            newWeapon = newRangeComponent;
        }

        return newWeapon;
    }

    private void CopyFields(object source, object target)
    {
        if (source == null || target == null) return;
        
        var currentType = source.GetType();
        // Поднимаемся по иерархии классов до MonoBehaviour, чтобы скопировать все приватные и публичные поля
        while (currentType != typeof(MonoBehaviour) && currentType != typeof(Behaviour) && currentType != typeof(Component) && currentType != typeof(Object) && currentType != null)
        {
            var fields = currentType.GetFields(System.Reflection.BindingFlags.Public | 
                                               System.Reflection.BindingFlags.NonPublic | 
                                               System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(target, field.GetValue(source));
                }
                catch
                {
                    // Игнорируем поля, которые не получается скопировать
                }
            }
            currentType = currentType.BaseType;
        }
    }
}