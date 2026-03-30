public class StateEnemyDead : State<Enemy>
{
    public StateEnemyDead(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Agent.isStopped = true;
        _character.Agent.enabled = false;
    }
}