using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerController : Character
{
    public Rigidbody _rb { get; private set; }
    public PlayerCombat Combat { get; private set; } 

    [Header("Movement Stats")]
    public float _moveSpeed = 5f; 
    public float _rotSpeed = 2f;

    [Header("Input")]
    public InputActionReference _move;
    public InputActionReference _shift;
    public InputActionReference _primeAttack;
    public InputActionReference _secondAttack;
    public InputActionReference _rotation;

    private StateMachine _SM;
    public StatePlayerMove _statePlayerMove;
    public StatePlayerMeleeAttack _statePlayerMeleeAttack;
    public StatePlayerRangeAttack _statePlayerRangeAttack;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        Combat = GetComponent<PlayerCombat>();
        _SM = new StateMachine();

        _statePlayerMeleeAttack = new StatePlayerMeleeAttack(this, _SM);
        _statePlayerMove = new StatePlayerMove(this, _SM);
        _statePlayerRangeAttack = new StatePlayerRangeAttack(this, _SM);

        _SM.Init(_statePlayerMove);
    }

    private void OnEnable()
    {
        _move.action.Enable(); _shift.action.Enable();
        _primeAttack.action.Enable(); _secondAttack.action.Enable(); _rotation.action.Enable();
    }

    private void OnDisable()
    {
        _move.action.Disable(); _shift.action.Disable();
        _primeAttack.action.Disable(); _secondAttack.action.Disable(); _rotation.action.Disable();
    }
    
    private void Update() => _SM._curState?.LogicUpdate();
    private void FixedUpdate() => _SM._curState?.PhysicsUpdate();
    
    // Вызывается из Animation Event
    public void OnAnimationEvent(AnimationEventType eventType) => _SM._curState?.OnAnimationEvent(eventType);

    public override void GetHit(int dmg, DamageType type)
    {
        if (_HP <= 0) return;
        base.GetHit(dmg, type);
        if (_HP > 0) _animator.SetTrigger("Hit");
    }
}