using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerView : MonoBehaviour, IGameOverTrigger, IHittable, IHealth, ITargetable
{
    [Header("Stats")]
    public int _MaxHP;
    public Animator _animator;

    [Header("Weapons")]
    [SerializeField] private GameObject _meleeWeaponObj;
    [SerializeField] private GameObject _rangeWeaponObj;
    [SerializeField] private GameObject _objectToToggle;

    public Rigidbody Rb { get; private set; }
    public IWeapon MeleeWeapon { get; private set; }
    public IWeapon RangeWeapon { get; private set; }

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference shiftAction;
    public InputActionReference primeAttackAction;
    public InputActionReference secondAttackAction;
    public InputActionReference rotationAction;
    public InputActionReference toggleAction;

    public PlayerModel Model { get; private set; }

    public event Action<Vector2, bool> OnMoveInputEvent;
    public event Action OnPrimeAttackEvent;
    public event Action OnSecondAttackEvent;
    public event Action OnToggleEvent;
    public event Action<int, DamageType> OnHitEvent;
    public event Action<AnimationEventType> OnAnimation;

    public void Initialize(PlayerModel model)
    {
        Model = model;
        Model.OnHealthChanged += (hp) => OnHealthChanged?.Invoke((float)hp / Model.MaxHealth);
        Model.OnDead += () => OnDeadEvent?.Invoke();
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        if (_meleeWeaponObj != null) MeleeWeapon = _meleeWeaponObj.GetComponent<IWeapon>();
        if (_rangeWeaponObj != null) RangeWeapon = _rangeWeaponObj.GetComponent<IWeapon>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        shiftAction.action.Enable();
        primeAttackAction.action.Enable();
        secondAttackAction.action.Enable();
        rotationAction.action.Enable();
        toggleAction.action.Enable();

        primeAttackAction.action.performed += _ => OnPrimeAttackEvent?.Invoke();
        secondAttackAction.action.performed += _ => OnSecondAttackEvent?.Invoke();
        toggleAction.action.performed += _ => OnToggleEvent?.Invoke();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        shiftAction.action.Disable();
        primeAttackAction.action.Disable();
        secondAttackAction.action.Disable();
        rotationAction.action.Disable();
        toggleAction.action.Disable();
    }

    public void DisableInput() => OnDisable();

    private void Update()
    {
        Vector2 moveDir = moveAction.action.ReadValue<Vector2>();
        bool isRunning = shiftAction.action.IsPressed();
        OnMoveInputEvent?.Invoke(moveDir, isRunning);
    }

    public void ToggleObject()
    {
        if (_objectToToggle != null) _objectToToggle.SetActive(!_objectToToggle.activeSelf);
    }

    public void OnAnimationEvent(AnimationEventType eventType)
    {
        OnAnimation?.Invoke(eventType);
    }


    public void GetHit(int dmg, DamageType type)
    {
        if (HP <= 0) return;
        OnHitEvent?.Invoke(dmg, type);
    }

    // IHealth
    public int MaxHP => Model?.MaxHealth ?? _MaxHP;
    public int HP => Model?.Health ?? _MaxHP;
    public event Action<float> OnHealthChanged;
    public float GetHealthNormalized() => (float)HP / MaxHP;

    // ITargetable
    public Vector3 TargetPosition => transform.position;
    public bool IsValidTarget => HP > 0;

    // IGameOverTrigger
    public event Action OnDeadEvent;
}
