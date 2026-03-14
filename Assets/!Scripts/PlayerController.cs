using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public StatePlayerRangeAttack _statePlayerRangeAttack;[Header("Range Attack & Cooldown")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;
    public float _magicCooldown = 2f;
    [HideInInspector] public float _lastMagicTime = -10f;
    public event Action<float> OnMagicCooldownChanged; // Событие для UI[Header("Melee Attack")]
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f; // Смещение зоны удара вперед

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

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
    
    private void Update()
    {
        if (Time.time < _lastMagicTime + _magicCooldown)
        {
            float progress = 1f - ((Time.time - _lastMagicTime) / _magicCooldown);
            OnMagicCooldownChanged?.Invoke(progress);
        }
        else
        {
            OnMagicCooldownChanged?.Invoke(0f);
        }
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

    public override void GetHit(int dmg, DamageType type)
    {
        base.GetHit(dmg, type);

        if (_HP <= 0)
        {
            GameController.Instance.GameLose();
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }

    public void MeleeDamageCheck()
    {
        // Создаем зону поражения перед игроком
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        Collider[] hitColliders = Physics.OverlapBox(hitCenter, _hitCube / 2, transform.rotation);
        
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == this.gameObject) continue; // Не бьем сами себя
            
            if (hit.TryGetComponent(out IHittable target))
            {
                target.GetHit(_dmg, DamageType.Melee); // Наносим физический урон
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        Gizmos.matrix = Matrix4x4.TRS(hitCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _hitCube);
    }
}