using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TargetScanner))]
public abstract class Enemy : Character
{
    [Header("Combat Settings")]
    public int _dmg; 
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent _agent { get; private set; }
    public TargetScanner Scanner { get; private set; }
    public ITargetable Target { get; set; } // DIP: Зависим от интерфейса

    protected StateMachine _SM;
    public StateEnemyChase _chaseState;
    public StateEnemyIdle _idleState;
    public IState _attackState; 
    public StateEnemyDead _deadState;

    protected override void Awake() 
    {
        base.Awake(); // LSP: Обязательный вызов базового метода
        _agent = GetComponent<NavMeshAgent>();
        Scanner = GetComponent<TargetScanner>();
        _SM = new StateMachine();
    }

    protected virtual void Update() => _SM._curState?.LogicUpdate();
    protected virtual void FixedUpdate() => _SM._curState?.PhysicsUpdate();
    
    public virtual void OnAnimationEvent(AnimationEventType eventType) => _SM._curState?.OnAnimationEvent(eventType);

    public override void GetHit(int dmg, DamageType type)
    {
        if (_HP <= 0) return; // LSP: Защита от двойной смерти
        
        base.GetHit(dmg, type);
        
        if (_HP <= 0) 
        {
            _SM.ChangeState(_deadState);
        }
    }

    public void OnDead() => Destroy(gameObject, 2.5f);
}