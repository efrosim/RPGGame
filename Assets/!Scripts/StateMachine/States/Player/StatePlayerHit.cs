using UnityEngine;

public class StatePlayerHit : State<PlayerView>
{
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private const float CrossFadeDuration = 0.1f;
    
    private float _stunTimer;
    private const float StunDuration = 0.4f; // Игроку стан можно сделать чуть короче

    public StatePlayerHit(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Rb.linearVelocity = Vector3.zero; // Сбрасываем физику (останавливаем)
        _character._animator.CrossFadeInFixedTime(HitHash, CrossFadeDuration);
        _stunTimer = 0f;
    }

    public override void LogicUpdate()
    {
        _stunTimer += Time.deltaTime;
        if (_stunTimer >= StunDuration)
        {
            _SM.ChangeState(new StatePlayerMove(_character, _SM));
        }
    }
}