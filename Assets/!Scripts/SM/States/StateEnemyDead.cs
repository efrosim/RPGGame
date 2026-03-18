public class StateEnemyDead : State<Enemy>
{
    public StateEnemyDead(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._agent.isStopped = true;
        _character._agent.enabled = false;
        // Уничтожение объекта теперь безопасно вызывается через событие OnDeadEvent в самом Enemy
    }
}