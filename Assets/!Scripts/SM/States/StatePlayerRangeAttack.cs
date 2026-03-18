public class StatePlayerRangeAttack : StatePlayerAttack
{
    public StatePlayerRangeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.DealDamage) 
        {
            _character.Combat.RangeAttackSheelCreate();
        }
        base.OnAnimationEvent(eventType);
    }
}