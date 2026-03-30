using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character, IGameOverTrigger
{
    
    [Header("Weapons")][Tooltip("Объект с компонентом, реализующим IWeapon (Ближний бой)")]
    [SerializeField] private GameObject _meleeWeaponObj;
    [Tooltip("Объект с компонентом, реализующим IWeapon (Дальний бой)")]
    [SerializeField] private GameObject _rangeWeaponObj;
    
    [Header("Toggle Object")] 
    [Tooltip("Объект, который будет включаться и выключаться по кнопке")] 
    [SerializeField] private GameObject _objectToToggle; 

    public Rigidbody _rb { get; private set; }
    public IWeapon Melee { get; private set; } 
    public IWeapon Range { get; private set; }
    public CooldownTimer MagicCooldown { get; private set; }

    [Header("Movement Stats")]
    public float _moveSpeed = 5f; 
    public float _rotSpeed = 2f;

    [Header("Input")]
    public InputActionReference _move;
    public InputActionReference _shift;
    public InputActionReference _primeAttack;
    public InputActionReference _secondAttack;
    public InputActionReference _rotation;
    public InputActionReference _toggleInput;

    private StateMachine _SM;
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        
        // Создаем чистый C# класс вместо GetComponent
        MagicCooldown = new CooldownTimer(2f); 
        
        if (_meleeWeaponObj != null) Melee = _meleeWeaponObj.GetComponent<IWeapon>();
        if (_rangeWeaponObj != null) Range = _rangeWeaponObj.GetComponent<IWeapon>();
        
        _SM = new StateMachine();

        AddState(new StatePlayerMove(this, _SM));
        AddState(new StatePlayerMeleeAttack(this, _SM));
        AddState(new StatePlayerRangeAttack(this, _SM));
        AddState(new StatePlayerHit(this, _SM)); 
        
        ChangeState<StatePlayerMove>();
    }

    private void AddState(IState state) => _states[state.GetType()] = state;

    public void ChangeState<T>() where T : IState
    {
        if (_states.TryGetValue(typeof(T), out IState state))
            _SM.ChangeState(state);
    }

    private void OnEnable()
    {
        _move.action.Enable(); 
        _shift.action.Enable();
        _primeAttack.action.Enable(); 
        _secondAttack.action.Enable(); 
        _rotation.action.Enable();
        _toggleInput.action.Enable(); 

        _primeAttack.action.performed += OnPrimeAttack;
        _secondAttack.action.performed += OnSecondAttack;
        _toggleInput.action.performed += OnToggleAction; 
    }

    private void OnDisable()
    {
        _primeAttack.action.performed -= OnPrimeAttack;
        _secondAttack.action.performed -= OnSecondAttack;
        _toggleInput.action.performed -= OnToggleAction; 

        _move.action.Disable(); 
        _shift.action.Disable();
        _primeAttack.action.Disable(); 
        _secondAttack.action.Disable(); 
        _rotation.action.Disable();
        _toggleInput.action.Disable();
    }

    public void DisableInput()
    {
        _move.action.Disable(); 
        _shift.action.Disable();
        _primeAttack.action.Disable(); 
        _secondAttack.action.Disable(); 
        _rotation.action.Disable();
        _toggleInput.action.Disable();
    }
    
    
    private void OnPrimeAttack(InputAction.CallbackContext ctx)
    {
        if (_SM._curState is StatePlayerMove)
            ChangeState<StatePlayerMeleeAttack>();
    }

    private void OnSecondAttack(InputAction.CallbackContext ctx)
    {
        if (_SM._curState is StatePlayerMove && MagicCooldown.IsReady)
        {
            MagicCooldown.StartCooldown();
            ChangeState<StatePlayerRangeAttack>();
        }
    }
    
    private void OnToggleAction(InputAction.CallbackContext ctx)
    {
        if (_objectToToggle != null)
        {
            _objectToToggle.SetActive(!_objectToToggle.activeSelf);
        }
        else
        {
            Debug.LogWarning("Объект для переключения не назначен в инспекторе!");
        }
    }
    
    private void Update() 
    {
        _SM.LogicUpdate();
        if (MagicCooldown != null)
        {
            MagicCooldown.Tick(Time.deltaTime);
        } 
    }
    
    private void FixedUpdate() => _SM.PhysicsUpdate();
    public void OnAnimationEvent(AnimationEventType eventType) => _SM.OnAnimationEvent(eventType);

    protected override void OnHitReceived(int dmg, DamageType type)
    {
        if (HP <= 0)
        {
            DisableInput();
        }
        else
        {
            ChangeState<StatePlayerHit>(); // Переходим в микро-стан
        }
    }
}