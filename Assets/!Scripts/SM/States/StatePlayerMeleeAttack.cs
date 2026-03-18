using System.Collections;
using UnityEngine;

public class StatePlayerMeleeAttack : StatePlayerAttack
{
    public StatePlayerMeleeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine)
    {
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
