using UnityEngine;

public class StatePlayerRangeAttack : StatePlayerAttack
{
    protected override int AttackHash => Animator.StringToHash("RangeAttack");

    public StatePlayerRangeAttack(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.DealDamage) 
            _character.RangeWeapon.Use();
        base.OnAnimationEvent(eventType);
    }
}