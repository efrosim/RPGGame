using UnityEngine;

public abstract class StateEnemyAttack<T> : State<T>, IAnimationState where T : Enemy
{
    protected abstract int AttackHash { get; }
    private const float CrossFadeDuration = 0.1f;

    public StateEnemyAttack(T character, StateMachine stateMachine) : base(character, stateMachine) { }
    
    public override void Enter()
    {
        _character.Agent.isStopped = true;
        _character._animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void LogicUpdate()
    {
        // Плавный поворот в сторону игрока во время атаки
        if (_character.Target != null)
        {
            Vector3 direction = _character.Target.TargetPosition - _character.transform.position;
            direction.y = 0; // Игнорируем разницу по высоте, чтобы враг не заваливался
            
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    public override void Exit() { }

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd)
            _character.ChangeState<StateEnemyChase>();
    }
}