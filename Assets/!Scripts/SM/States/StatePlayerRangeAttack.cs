using UnityEngine;

public class StatePlayerRangeAttack : StatePlayerAttack
{
    public StatePlayerRangeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        OnShellFire();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void EventHandler(AnimEnums animstate)
    {
        base.EventHandler(animstate);
    }
    public override void LogicUpdate()
    {

    }
    public override void Update()
    {

    }

    private void OnShellFire()
    {
        _character.RangeAttackSheelCreate();
    }
}
