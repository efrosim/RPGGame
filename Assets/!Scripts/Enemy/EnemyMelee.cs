public class EnemyMelee : Enemy
{
    public IWeapon Melee { get; private set; }

    protected override void Awake()
    {
        base.Awake(); 

        Melee = GetComponent<IWeapon>();

        AddState(new StateEnemyChase(this, _SM));
        AddState(new StateEnemyIdle(this, _SM));
        AddState(new StateEnemyMeleeAttack(this, _SM));
        AddState(new StateEnemyDead(this, _SM));
        AddState(new StateEnemyHit(this, _SM)); 
        
        ChangeState<StateEnemyIdle>();
    }

    public override void TransitionToAttackState() => ChangeState<StateEnemyMeleeAttack>();
}