using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TargetScanner))]
public abstract class Enemy : Character
{
    public string UniqueId { get; private set; }

    [Header("Combat Settings")]
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent Agent { get; private set; }
    public TargetScanner Scanner { get; private set; }
    public ITargetable Target { get; set; } 

    protected StateMachine _SM;
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

    protected override void Awake() 
    {
        base.Awake(); 
        Agent = GetComponent<NavMeshAgent>();
        Scanner = GetComponent<TargetScanner>();
        _SM = new StateMachine();
        
        UniqueId = $"{gameObject.name}_{transform.position.ToString("F2")}";
    }
    
    public void SetDynamicId(string newId)
    {
        UniqueId = newId;
    }

    protected override void Start()
    {
        base.Start();
        OnDeadEvent += HandleDeath;
    }

    protected void AddState(IState state) => _states[state.GetType()] = state;

    public void ChangeState<T>() where T : IState
    {
        if (_states.TryGetValue(typeof(T), out IState state))
            _SM.ChangeState(state);
    }

    public abstract void TransitionToAttackState();

    protected virtual void Update() => _SM.LogicUpdate();
    protected virtual void FixedUpdate() => _SM.PhysicsUpdate();
    public virtual void OnAnimationEvent(AnimationEventType eventType) => _SM.OnAnimationEvent(eventType);

    protected override void OnHitReceived(int dmg, DamageType type)
    {
        if (_HP <= 0) 
            ChangeState<StateEnemyDead>();
        else 
            ChangeState<StateEnemyHit>(); 
    }

    private void HandleDeath()
    {
        Destroy(gameObject, 2.5f);
    }
}