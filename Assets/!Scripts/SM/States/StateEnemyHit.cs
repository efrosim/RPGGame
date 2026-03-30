using UnityEngine;

public class StateEnemyHit : State<Enemy>
{
    private static readonly int HitHash = Animator.StringToHash("Hit"); // Имя анимации получения урона
    private const float CrossFadeDuration = 0.1f;
    
    private float _stunTimer;
    private const float StunDuration = 0.5f; // Длительность микро-стана в секундах

    public StateEnemyHit(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Agent.isStopped = true; // Останавливаем врага
        _character._animator.CrossFadeInFixedTime(HitHash, CrossFadeDuration);
        _stunTimer = 0f;
    }

    public override void LogicUpdate()
    {
        _stunTimer += Time.deltaTime;
        if (_stunTimer >= StunDuration)
        {
            // После стана враг сразу переходит в погоню (агрится)
            _character.ChangeState<StateEnemyChase>();
        }
    }
}