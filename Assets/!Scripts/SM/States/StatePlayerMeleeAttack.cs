using System.Collections;
using UnityEngine;

public class StatePlayerMeleeAttack : State
{
    private new PlayerController _character;
    public StatePlayerMeleeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (PlayerController)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        Attack();
    }
    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }
    public override void EventHandler(AnimEnums animstate)
    {
        _SM.ChangeState(_character._statePlayerMove);
        _character._isAttackFinish = false;
    }
    public override void LogicUpdate()
    {
        
    }
    public override void Update()
    {
        
    }

    private void Attack()
    {
        if (_character._primeAttack.action.IsPressed())
        {
            _character._animator.SetBool("IsAttack", true);

            Debug.Log("LMB");
        }

        if (_character._secondAttack.action.IsPressed())
        {
            

            Debug.Log("RMB");
        }
    }
}
