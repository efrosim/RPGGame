public class EnemyRange : Enemy
{
    public IWeapon Range { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Range = GetComponent<IWeapon>();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyRangeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));

        ChangeState<StateEnemyIdle>();
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyRangeAttack>();
}