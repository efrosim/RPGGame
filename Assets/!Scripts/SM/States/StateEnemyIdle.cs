using UnityEngine;

public class StateEnemyIdle : State<Enemy>
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private const float CrossFadeDuration = 0.1f;
    private float _scanTimer;

    public StateEnemyIdle(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }
    
    public override void Enter() 
    {
        _character.Agent.isStopped = true;
        _character._animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }

    public override void LogicUpdate()
    {
        _scanTimer += Time.deltaTime;
        if (_scanTimer >= 0.2f) 
        {
            _scanTimer = 0f;
            _character.Target = _character.Scanner.Scan();
        }

        if (_character.Target != null)
        {
            _character.ChangeState<StateEnemyChase>();
        }
    }
}