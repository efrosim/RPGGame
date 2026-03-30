using UnityEngine;

public class StateEnemyIdle : State<Enemy>
{
    public StateEnemyIdle(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    private float _scanTimer;
    
    public override void Enter() => _character.Agent.isStopped = true;

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