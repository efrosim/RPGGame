public class StateEnemyRangeAttack : StateEnemyAttack<EnemyRange>
{
    public StateEnemyRangeAttack(EnemyRange character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if(eventType == AnimationEventType.DealDamage) 
            _character.Range.Shoot();
        base.OnAnimationEvent(eventType);
    }
}