using System.Collections;
using UnityEngine;

public class StatePlayerAttack : State<PlayerController>
{
    public StatePlayerAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        _character._animator.SetBool("IsAttack", true);
        
    }
    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }
    public override void EventHandler(AnimEnums animstate)
    {
        _SM.ChangeState(_character._statePlayerMove);
    }
    public override void LogicUpdate()
    {

    }
    public override void Update()
    {

    }
}
