using UnityEngine;

// Убираем RequireComponent конкретного оружия
public class EnemyMelee : Enemy
{
    // Меняем тип на интерфейс
    public IWeapon Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake(); 
        // Получаем любой компонент, который реализует IWeapon
        Melee = GetComponent<IWeapon>();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyMeleeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));

        ChangeState<StateEnemyIdle>();
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyMeleeAttack>();
}