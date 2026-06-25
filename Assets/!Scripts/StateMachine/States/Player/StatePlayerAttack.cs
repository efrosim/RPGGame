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
        _character.OnAnimation += OnAnimationEvent;
    }

    public override void Exit() { _character.OnAnimation -= OnAnimationEvent; }

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) 
        {
            Vector2 input = _character.MovementInput;
            if (input.sqrMagnitude > 0.001f)
                _SM.ChangeState(new StatePlayerMove(_character, _SM));
            else
                _SM.ChangeState(new StatePlayerIdle(_character, _SM));
        }
    }
}