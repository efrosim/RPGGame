using System.Collections;
using UnityEngine;

public class StatePlayerAttack : State
{
    private new PlayerController _character;
    public StatePlayerAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (PlayerController)character;
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
