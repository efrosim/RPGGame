public class EnemyRange : Enemy
{
    public IWeapon Range { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyRangeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));
        AddState(new StateEnemyHit(this, _SM)); 
        AddState(new StateEnemyFlee(this, _SM));
        
        if(TryGetComponent(out RangeWeapon component)) Range = component;
    }

    protected override void Start()
    {
        base.Start();
        ChangeState<StateEnemyIdle>();
    }

    public override void InitWeapon(IWeapon weapon)
    {
        Range = weapon;
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyRangeAttack>();
}