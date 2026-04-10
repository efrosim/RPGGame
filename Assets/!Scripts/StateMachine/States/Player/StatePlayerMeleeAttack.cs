using UnityEngine;

public class StatePlayerMeleeAttack : StatePlayerAttack
{
    protected override int AttackHash => Animator.StringToHash("MeleeAttack");

    public StatePlayerMeleeAttack(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.DealDamage)
            _character.MeleeWeapon.Use(); 
        base.OnAnimationEvent(eventType); 
    }
}