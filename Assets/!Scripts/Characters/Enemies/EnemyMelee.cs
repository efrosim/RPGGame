public class EnemyMelee : Enemy
{
    public IWeapon Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake(); 

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyMeleeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));
        AddState(new StateEnemyHit(this, _SM)); 
        AddState(new StateEnemyFlee(this, _SM));
        
        ChangeState<StateEnemyIdle>();

        if (TryGetComponent(out MeleeWeapon component)) Melee = component;
    }

    public override void InitWeapon(IWeapon weapon)
    {
        Melee = weapon;
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyMeleeAttack>();
}