using UnityEngine;

public class StatePlayerHit : State<PlayerController>
{
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private const float CrossFadeDuration = 0.1f;
    
    private float _stunTimer;
    private const float StunDuration = 0.4f; // Игроку стан можно сделать чуть короче

    public StatePlayerHit(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._rb.linearVelocity = Vector3.zero; // Сбрасываем физику (останавливаем)
        _character._animator.CrossFadeInFixedTime(HitHash, CrossFadeDuration);
        _stunTimer = 0f;
    }

    public override void LogicUpdate()
    {
        _stunTimer += Time.deltaTime;
        if (_stunTimer >= StunDuration)
        {
            _character.ChangeState<StatePlayerMove>();
        }
    }
}