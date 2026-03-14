using System.Collections;
using UnityEngine;

public class StatePlayerMeleeAttack : StatePlayerAttack
{
    private new PlayerController _character;
    public StatePlayerMeleeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (PlayerController)character;
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
        OnDealDmg();
    }
    public override void LogicUpdate()
    {
        
    }
    public override void Update()
    {
        
    }

    private void OnDealDmg()
    {
        _character.MeleeDamageCheck();
    }
}
