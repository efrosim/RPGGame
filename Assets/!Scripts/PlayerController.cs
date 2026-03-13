using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Character
{
    public static PlayerController Instance;

    public Rigidbody _rb;

    [Header("Input")]
    public InputActionReference _move;
    public InputActionReference _shift;
    public InputActionReference _primeAttack;
    public InputActionReference _secondAttack;
    public InputActionReference _rotation;

    [Header("States")]
    private StateMachine _SM;
    public StatePlayerMove _statePlayerMove;
    public StatePlayerMeleeAttack _statePlayerMeleeAttack;
    public StatePlayerRangeAttack _statePlayerRangeAttack;

    [Header("Range Attack")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    [Header("Other")]
    [SerializeField] private Vector3 _hitCube;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        _rb = GetComponent<Rigidbody>();

        _SM = new StateMachine();

        _statePlayerMeleeAttack = new StatePlayerMeleeAttack(this, _SM);
        _statePlayerMove = new StatePlayerMove(this, _SM);
        _statePlayerRangeAttack = new StatePlayerRangeAttack(this, _SM);

        _SM.Init(_statePlayerMove);
    }
    private void OnEnable()
    {
        _move.action.Enable();
        _shift.action.Enable();
        _primeAttack.action.Enable();
        _secondAttack.action.Enable();
        _rotation.action.Enable();
    }
    private void OnDisable()
    {
        _move.action.Disable();
        _shift.action.Disable();
        _primeAttack.action.Disable();
        _secondAttack.action.Disable();
        _rotation.action.Disable();
    }
    
    private void FixedUpdate()
    {
        _SM._curState.LogicUpdate();
        _SM._curState.Update();
    }

    public void EventHandler(AnimEnums state)
    {
        _SM._curState.EventHandler(state);
    }

    public override void GetHit(int dmg)
    {
        base.GetHit(dmg);
        if (_HP <= 0) GameController.Instance.GameLose();
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }

    public void MeleeDamageCheck()
    {
        var hitColliders = Physics.OverlapBox(transform.position, _hitCube);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, _hitCube);
    //}
}
