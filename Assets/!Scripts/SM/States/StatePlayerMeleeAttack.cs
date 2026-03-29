public class StatePlayerMeleeAttack : StatePlayerAttack
{
    public StatePlayerMeleeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.DealDamage)
            _character.Melee.Use(); // ИСПРАВЛЕНО: вызываем Use() из интерфейса IWeapon
        base.OnAnimationEvent(eventType); 
    }
}