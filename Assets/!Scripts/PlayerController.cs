using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Character
{
    private Rigidbody _rb;
    private bool _onReload = false;

    [Header("Input")]
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _shift;
    [SerializeField] private InputActionReference _primeAttack;
    [SerializeField] private InputActionReference _secondAttack;
    [SerializeField] private InputActionReference _rotation;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
    protected override void Attack()
    {
        if(_onReload) return;

        if (_primeAttack.action.IsPressed())
        {
            _onReload = true;
            StartCoroutine(ReloadTimer());

            Debug.Log("LMB");
        }

        if (_secondAttack.action.IsPressed())
        {
            _onReload = true;
            StartCoroutine(ReloadTimer());

            Debug.Log("RMB"); 
        }
    }
    private void Rotation()
    {
        transform.Rotate(Vector3.up, _rotation.action.ReadValue<float>() * _rotSpeed, Space.World);
    }
    private void Movement()
    {
        if(_move.action.IsPressed())
        {
            float speedMod = 1f;
            if (_shift.action.IsPressed())
                speedMod = 1.5f;

            Vector2 input = _move.action.ReadValue<Vector2>();
            Vector3 dir = transform.forward * input.y * speedMod + transform.right * input.x * speedMod;

            _rb.AddForce(dir * _moveSpeed, ForceMode.Force);
        }
    }
    private void FixedUpdate()
    {
        Rotation();
        Movement();
        Attack();
    }
    protected override void Dead()
    {
        return;
    }
    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(_reload);
        _onReload = false;
        Debug.Log("End timer");
    }
}
