using UnityEngine;

[RequireComponent(typeof(RangeWeapon))]
public class EnemyRange : Enemy
{
    public RangeWeapon Range { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Range = GetComponent<RangeWeapon>();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyRangeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));

        ChangeState<StateEnemyIdle>();
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyRangeAttack>();
}