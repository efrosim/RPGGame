using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent _agent;

    [Header("State Machine")]
    protected StateMachine _SM;
    public StateEnemyChase _chaseState;
    public StateEnemyIdle _idleState;
    public StateEnemyAttack _attackState;
    public StateEnemyDead _deadState;
    
    protected virtual void FixedUpdate()
    {
        _SM._curState.LogicUpdate();
        _SM._curState.Update();
    }

    public virtual void EventHandler(AnimEnums state)
    {
        _SM._curState.EventHandler(state);
    }

    public override void GetHit(int dmg, DamageType type)
    {
        base.GetHit(dmg, type);

        if (_HP <= 0)
        {
            _SM.ChangeState(_deadState);
        }
    }

    public void OnDead()
    {
        Destroy(gameObject, 2.5f);
    }
}