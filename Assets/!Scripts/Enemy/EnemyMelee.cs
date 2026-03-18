using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
public class EnemyMelee : Enemy
{
    public MeleeWeapon Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake(); 
        Melee = GetComponent<MeleeWeapon>();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyMeleeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));

        ChangeState<StateEnemyIdle>();
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyMeleeAttack>();
}