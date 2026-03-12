using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    [Header("State Machine")]
    public StateEnemyMeleeAttack _meleeAttackState;

    protected void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyMeleeAttack(this, _SM);

        _SM.Init(_idleState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void EventHandler(AnimEnums state)
    {
        base.EventHandler(state);
    }
}

