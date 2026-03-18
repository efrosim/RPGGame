public class StateEnemyMeleeAttack : StateEnemyAttack<EnemyMelee>
{
    public StateEnemyMeleeAttack(EnemyMelee character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if(eventType == AnimationEventType.DealDamage) 
        {
            _character.DealDmg();
        }
        base.OnAnimationEvent(eventType);
    }
}