using UnityEngine;

public class StateEnemyRangeAttack : StateEnemyAttack<EnemyRange>
{
    public StateEnemyRangeAttack(EnemyRange character, StateMachine stateMachine) : base(character, stateMachine)
    {
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
        // _character автоматически EnemyRange
        _character.RangeAttackSheelCreate();
    }

    private void OnAttackEnd()
    {
        _SM.ChangeState(_character._chaseState);
    }
}
