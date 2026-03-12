using UnityEngine;

public class StateEnemyMeleeAttack : StateEnemyAttack
{
    private new EnemyMelee _character;
    public StateEnemyMeleeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (EnemyMelee)character;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void EventHandler(AnimEnums animstate)
    {
        base.EventHandler(animstate);

        if(animstate == AnimEnums.DealDmg) OnDealDmg();
    }

    public override void LogicUpdate()
    {

    }

    private void OnDealDmg()
    {
        if (Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position) < _character._attackRange)
        {
            _character.DealDmg();
        }
    }
}
