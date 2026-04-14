using UnityEngine;

public abstract class StatePlayerAttack : State<PlayerView>, IAnimationState
{
    protected abstract int AttackHash { get; }
    private const float CrossFadeDuration = 0.1f;

    public StatePlayerAttack(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() 
    {
        _character.Rb.linearVelocity = Vector3.zero;
        _character._animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
        _character.OnAnimation += (eventType) => OnAnimationEvent(eventType);
    }

    public override void Exit() { _character.OnAnimation -= (eventType) => OnAnimationEvent(eventType); }

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) 
            _SM.ChangeState(new StatePlayerMove(_character, _SM));
    }
}