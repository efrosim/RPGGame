using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    protected void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _deadState = new StateEnemyDead(this, _SM);

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
    
    public override void DealDmg()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.GetHit(_dmg, DamageType.Melee);
        }
    }
}

