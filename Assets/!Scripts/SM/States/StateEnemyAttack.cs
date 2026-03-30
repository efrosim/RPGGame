using UnityEngine;

public abstract class StateEnemyAttack<T> : State<T>, IAnimationState where T : Enemy
{
    // Абстрактное свойство, которое обяжет наследников передать хэш своей анимации
    protected abstract int AttackHash { get; }
    private const float CrossFadeDuration = 0.1f;

    public StateEnemyAttack(T character, StateMachine stateMachine) : base(character, stateMachine) { }
    
    public override void Enter()
    {
        _character.Agent.isStopped = true;
        // Жестко приказываем Аниматору включить нужную анимацию атаки
        _character._animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit() 
    { 
        // SetBool больше не нужен!
    }

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd)
            _character.ChangeState<StateEnemyChase>();
    }
}