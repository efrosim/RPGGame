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
    }
    public override void Exit()
    {

    }
    public override void LogicUpdate()
    {

    }
    public override void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (_character._onReload) return;

        if (_character._primeAttack.action.IsPressed())
        {
            _character._onReload = true;

            Debug.Log("LMB");
        }

        if (_character._secondAttack.action.IsPressed())
        {
            _character._onReload = true;
            

            Debug.Log("RMB");
        }
    }
}
