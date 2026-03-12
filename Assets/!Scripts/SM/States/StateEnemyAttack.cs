using UnityEngine;

public class StateEnemyAttack : State
{
    private new Enemy _character;
    public StateEnemyAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = true;
        _character._animator.SetBool("IsAttack", true);
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }

    public override void EventHandler(AnimEnums animstate)
    {
        if (animstate == AnimEnums.AttackEnd) OnAttackEnd();
    }

    public override void LogicUpdate()
    {

    }

    private void OnAttackEnd()
    {
        _SM.ChangeState(_character._chaseState);
    }
}

