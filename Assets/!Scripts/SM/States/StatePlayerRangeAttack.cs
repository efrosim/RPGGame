using UnityEngine;

public class StatePlayerRangeAttack : StatePlayerAttack
{
    private new PlayerController _character;
    public StatePlayerRangeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (PlayerController)character;
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
