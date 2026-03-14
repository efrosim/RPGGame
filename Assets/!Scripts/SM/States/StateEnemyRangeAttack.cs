using UnityEngine;

public class StateEnemyRangeAttack : StateEnemyAttack
{
    private new EnemyRange _character;
    public StateEnemyRangeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (EnemyRange)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = true;
        _character._animator.SetBool("IsAttack", true);
        OnShellFire();
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }

    public override void EventHandler(AnimEnums animstate)
    {
        if(animstate == AnimEnums.AttackEnd) OnAttackEnd();
    }

    public override void LogicUpdate()
    {

    }

    private void OnShellFire()
    {
        _character.RangeAttackSheelCreate();
    }

    private void OnAttackEnd()
    {
        _SM.ChangeState(_character._chaseState);
    }
}
