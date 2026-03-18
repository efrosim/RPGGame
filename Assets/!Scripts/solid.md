


С удовольствием! Мы сейчас проведем **рефакторинг** (улучшение кода без изменения его работы для игрока). Суть игры, механики и логика останутся ровно теми же, но код станет профессиональным, гибким и будет соответствовать принципам **SOLID**.

Я разобью изменения на 4 логических шага и объясню каждый из них. В конце я дам готовые скрипты для копирования.

---

### Шаг 1. Избавляемся от `AnimEnums` (Принцип OCP — Открытости/Закрытости)
**Проблема:** Каждый раз, когда мы добавляем новую анимацию, нам нужно изменять файл `StatesEnum.cs` и дописывать код в состояниях. Это нарушает принцип OCP (код должен быть закрыт для изменения, но открыт для расширения).
**Решение:** Мы удаляем `AnimEnums` и заменяем его на обычные **строки (`string`)**. Теперь ты сможешь добавлять любые события в Unity (например, `"AttackEnd"`, `"DealDmg"`, `"Step"`), и код автоматически их поймет без переписывания.

*⚠️ Важно: После этого изменения тебе нужно будет зайти в Unity, открыть свои анимации (окно Animation), нажать на твои Animation Events и в поле `String` вписать названия событий (например, `AttackEnd` или `DealDmg`), так как старые Enum-ивенты сбросятся.*

### Шаг 2. Убираем пустой `DealDmg` (Принцип LSP — Подстановки Лисков)
**Проблема:** В базовом классе `Character` есть метод `DealDmg()`, который нужен только врагу ближнего боя. Игрок и враг дальнего боя его игнорируют. Это нарушает контракт базового класса.
**Решение:** Мы удаляем `DealDmg()` из `Character` и оставляем его только внутри `EnemyMelee`, где он действительно нужен.

### Шаг 3. Избавляемся от жестких связей / Синглтонов (Принцип DIP — Инверсии зависимостей)
**Проблема:** Враги жестко привязаны к `PlayerController.Instance`. Если ты захочешь добавить второго игрока или приманку, придется переписывать врагов. А игрок жестко привязан к `GameController.Instance.GameLose()`.
**Решение:** 
1. Мы дадим врагам переменную `Transform Target`. При старте игры они сами найдут цель и будут бегать за ней. Состояния врагов больше ничего не знают про `PlayerController`.
2. В `Character` мы добавим событие `public event Action OnDeadEvent;`. Когда ХП падает до нуля, игрок просто кричит: *"Я умер!"*. А `GameController` подписывается на этот крик и сам включает экран проигрыша. Игрок больше не управляет игрой.

### Шаг 4. Разделяем физику и ввод (Принцип SRP — Единственной обязанности)
**Проблема:** Чтение кнопок (`LogicUpdate`) происходит в `FixedUpdate` (физика). Из-за этого теряются клики. Кроме того, метод в интерфейсе называется `Update`, но вызывается в `FixedUpdate`, что путает.
**Решение:** Переименуем `Update` в `PhysicsUpdate` в интерфейсе состояний. В `PlayerController` и `Enemy` разнесем логику по правильным методам Unity.

---

### 📋 ГОТОВЫЕ СКРИПТЫ (Скопируй и замени свои)

#### 1. Ядро состояний (Интерфейсы и База)
Файл `StatesEnum.cs` **МОЖНО УДАЛИТЬ**. Он больше не нужен.

**`IState.cs`**
```csharp
public interface IState
{
    void Enter();
    void Exit();
    void EventHandler(string eventName); // OCP: Теперь используем строки вместо Enum
    void LogicUpdate();
    void PhysicsUpdate(); // SRP: Переименовали, чтобы было понятно, что это для FixedUpdate
}
```

**`StateMachine.cs`**
```csharp
public class StateMachine
{
    public IState _curState;

    public void Init(IState startState)
    {
        _curState = startState;
        _curState.Enter();
    }

    public void ChangeState(IState newState)
    {
        _curState?.Exit();
        _curState = newState;
        _curState.Enter();
    }
}
```

**`State.cs`**
```csharp
public abstract class State<T> : IState where T : Character 
{
    protected T _character;
    protected StateMachine _SM;

    public State(T character, StateMachine stateMachine)
    {
        _character = character;
        _SM = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void EventHandler(string eventName) { } // OCP: Строки
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { } // SRP: Физика
}
```

#### 2. Базовые классы и Контроллеры

**`Character.cs`**
```csharp
using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IHittable, IHealth
{
    [Header("Stats")][SerializeField] protected int _HP;
    [SerializeField] protected int _MaxHP;
    [SerializeField] protected int _dmg;
    public float _moveSpeed;
    public float _rotSpeed;

    public Animator _animator;

    public int HP => _HP;
    public int MaxHP => _MaxHP;

    public event Action<float> OnHealthChanged; 
    public event Action OnDeadEvent; // DIP: Событие смерти вместо жесткой связи с GameController
    
    protected virtual void Start()
    {
        _HP = _MaxHP;
        OnHealthChanged?.Invoke(GetHealthNormalized()); 
    }

    public virtual void GetHit(int dmg, DamageType type)
    {
        _HP -= dmg;
        OnHealthChanged?.Invoke(GetHealthNormalized());

        if (_HP <= 0)
        {
            OnDeadEvent?.Invoke(); // Сообщаем всем, кто подписан, что персонаж умер
        }
    }

    // LSP: Удалили пустой DealDmg(), так как он нужен не всем персонажам

    public float GetHealthNormalized()
    {
        if (_MaxHP == 0) return 0f; 
        return (float)_HP / _MaxHP;
    }
}
```

**`GameController.cs`**
```csharp
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private GameObject _restartCanvas;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        ResumeGame();
    }

    private void Start()
    {
        // DIP: GameController сам находит игрока и подписывается на его смерть.
        // Игрок больше ничего не знает про GameController.
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnDeadEvent += GameLose;
        }
    }

    public void GameLose()
    {
        PauseGame();
        _restartCanvas.SetActive(true);
    }

    public void PauseGame() => Time.timeScale = 0.0f;
    public void ResumeGame() => Time.timeScale = 1.0f;
}
```

**`PlayerController.cs`**
```csharp
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Character
{
    public static PlayerController Instance;

    public Rigidbody _rb;[Header("Input")]
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

    [Header("Range Attack & Cooldown")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;
    public float _magicCooldown = 2f;[HideInInspector] public float _lastMagicTime = -10f;
    public event Action<float> OnMagicCooldownChanged;

    [Header("Melee Attack")]
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

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

        // SRP: Чтение ввода и логика переходов теперь в Update! (Кнопки не будут теряться)
        _SM._curState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        // SRP: Физика осталась в FixedUpdate
        _SM._curState?.PhysicsUpdate();
    }

    public void EventHandler(string eventName)
    {
        _SM._curState?.EventHandler(eventName);
    }

    public override void GetHit(int dmg, DamageType type)
    {
        base.GetHit(dmg, type); // Здесь вызывается OnDeadEvent, если ХП <= 0

        if (_HP > 0)
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
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        Collider[] hitColliders = Physics.OverlapBox(hitCenter, _hitCube / 2, transform.rotation);
        
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject == this.gameObject) continue;
            
            if (hit.TryGetComponent(out IHittable target))
            {
                target.GetHit(_dmg, DamageType.Melee);
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
```

#### 3. Враги (База и Наследники)

**`Enemy.cs`**
```csharp
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent _agent;
    
    // DIP: Враг теперь имеет абстрактную цель, а не жесткую ссылку на PlayerController.Instance
    public Transform Target { get; protected set; }[Header("State Machine")]
    protected StateMachine _SM;
    public StateEnemyChase _chaseState;
    public StateEnemyIdle _idleState;
    public IState _attackState; 
    public StateEnemyDead _deadState;

    protected virtual void Awake()
    {
        // Простая инъекция зависимости: находим цель при рождении
        if (PlayerController.Instance != null)
        {
            Target = PlayerController.Instance.transform;
        }
    }

    protected virtual void Update()
    {
        // SRP: Логика в Update
        _SM._curState?.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        // SRP: Физика в FixedUpdate
        _SM._curState?.PhysicsUpdate();
    }

    public virtual void EventHandler(string eventName)
    {
        _SM._curState?.EventHandler(eventName);
    }

    public override void GetHit(int dmg, DamageType type)
    {
        base.GetHit(dmg, type);

        if (_HP <= 0)
        {
            _SM.ChangeState(_deadState);
        }
    }

    public void OnDead()
    {
        Destroy(gameObject, 2.5f);
    }
}
```

**`EnemyMelee.cs`**
```csharp
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    protected override void Awake()
    {
        base.Awake(); // Обязательно вызываем базовый Awake, чтобы найти Target

        _agent = GetComponent<NavMeshAgent>();
        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyMeleeAttack(this, _SM);
        _deadState = new StateEnemyDead(this, _SM);

        _SM.Init(_idleState);
    }

    // LSP: Метод DealDmg теперь живет только здесь, где он реально нужен
    public void DealDmg()
    {
        if (Target != null && Target.TryGetComponent(out IHittable hittable))
        {
            hittable.GetHit(_dmg, DamageType.Melee);
        }
    }
}
```

**`EnemyRange.cs`**
```csharp
using UnityEngine;
using UnityEngine.AI;

public class EnemyRange : Enemy
{
    [Header("Range Attack")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    protected override void Awake()
    {
        base.Awake(); // Находим Target

        _agent = GetComponent<NavMeshAgent>();
        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyRangeAttack(this, _SM);
        _deadState = new StateEnemyDead(this, _SM);

        _SM.Init(_idleState);
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }
}
```

#### 4. Состояния Игрока

**`StatePlayerMove.cs`**
```csharp
using UnityEngine;

public class StatePlayerMove : State<PlayerController> 
{
    public StatePlayerMove(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void LogicUpdate()
    {
        if(_character._primeAttack.action.WasPressedThisFrame())
        {
            _SM.ChangeState(_character._statePlayerMeleeAttack);
        }
        
        if(_character._secondAttack.action.WasPressedThisFrame() && Time.time >= _character._lastMagicTime + _character._magicCooldown)
        {
            _character._lastMagicTime = Time.time; 
            _SM.ChangeState(_character._statePlayerRangeAttack);
        }
    }

    public override void PhysicsUpdate() // SRP: Переименовали
    {
        Rotation();
        Movement();
    }

    private void Rotation()
    {
        _character.transform.Rotate(Vector3.up, _character._rotation.action.ReadValue<float>() * _character._rotSpeed, Space.World);
    }
    
    private void Movement()
    {
        Vector2 input = _character._move.action.ReadValue<Vector2>();
        float speedMod = _character._shift.action.IsPressed() ? 1.5f : 1f;

        Vector3 dir = _character.transform.forward * input.y + _character.transform.right * input.x;
        if (dir.magnitude > 1f) dir.Normalize();

        Vector3 targetVelocity = dir * (_character._moveSpeed * speedMod);
        _character._rb.linearVelocity = new Vector3(targetVelocity.x, _character._rb.linearVelocity.y, targetVelocity.z);
    }
}
```

**`StatePlayerAttack.cs`**
```csharp
public class StatePlayerAttack : State<PlayerController>
{
    public StatePlayerAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._animator.SetBool("IsAttack", true);
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }

    public override void EventHandler(string eventName)
    {
        if (eventName == "AttackEnd") // OCP: Проверяем строку
        {
            _SM.ChangeState(_character._statePlayerMove);
        }
    }
}
```

**`StatePlayerMeleeAttack.cs`**
```csharp
public class StatePlayerMeleeAttack : StatePlayerAttack
{
    public StatePlayerMeleeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void EventHandler(string eventName)
    {
        if (eventName == "DealDmg")
        {
            _character.MeleeDamageCheck();
        }
        base.EventHandler(eventName); // Передаем дальше, чтобы сработал AttackEnd
    }
}
```

**`StatePlayerRangeAttack.cs`**
```csharp
public class StatePlayerRangeAttack : StatePlayerAttack
{
    public StatePlayerRangeAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void EventHandler(string eventName)
    {
        if (eventName == "DealDmg") // Снаряд вылетает по ивенту анимации
        {
            _character.RangeAttackSheelCreate();
        }
        base.EventHandler(eventName);
    }
}
```

#### 5. Состояния Врагов

**`StateEnemyChase.cs`**
```csharp
using UnityEngine;

public class StateEnemyChase : State<Enemy>
{
    public StateEnemyChase(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._agent.isStopped = false;
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsChase", false);
    }

    public override void LogicUpdate()
    {
        if (_character.Target == null) return; // DIP: Проверяем абстрактную цель

        float distanceToPlayer = Vector3.Distance(_character.transform.position, _character.Target.position);

        if (distanceToPlayer > _character._idleRange)
            _SM.ChangeState(_character._idleState);
        else if (distanceToPlayer <= _character._attackRange)
            _SM.ChangeState(_character._attackState);
    }
    
    public override void PhysicsUpdate()
    {
        if (_character.Target == null) return;

        _character._agent.destination = _character.Target.position;
        _character._animator.SetBool("IsChase", _character._agent.velocity.sqrMagnitude >= 0.01f);
    }
}
```

**`StateEnemyIdle.cs`**
```csharp
using UnityEngine;

public class StateEnemyIdle : State<Enemy>
{
    public StateEnemyIdle(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._agent.isStopped = true;
    }

    public override void LogicUpdate()
    {
        if (_character.Target == null) return;

        if (Vector3.Distance(_character.transform.position, _character.Target.position) < _character._idleRange)
        {
            _SM.ChangeState(_character._chaseState);
        }
    }
}
```

**`StateEnemyAttack.cs`**
```csharp
public class StateEnemyAttack<T> : State<T> where T : Enemy
{
    public StateEnemyAttack(T character, StateMachine stateMachine) : base(character, stateMachine) { }
    
    public override void Enter()
    {
        _character._agent.isStopped = true;
        _character._animator.SetBool("IsAttack", true);
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsAttack", false);
    }

    public override void EventHandler(string eventName)
    {
        if (eventName == "AttackEnd")
        {
            _SM.ChangeState(_character._chaseState);
        }
    }
}
```

**`StateEnemyMeleeAttack.cs`**
```csharp
using UnityEngine;

public class StateEnemyMeleeAttack : StateEnemyAttack<EnemyMelee>
{
    public StateEnemyMeleeAttack(EnemyMelee character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void EventHandler(string eventName)
    {
        if(eventName == "DealDmg") 
        {
            if (_character.Target != null && Vector3.Distance(_character.transform.position, _character.Target.position) < _character._attackRange)
            {
                _character.DealDmg();
            }
        }
        base.EventHandler(eventName);
    }
}
```

**`StateEnemyRangeAttack.cs`**
```csharp
public class StateEnemyRangeAttack : StateEnemyAttack<EnemyRange>
{
    public StateEnemyRangeAttack(EnemyRange character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void EventHandler(string eventName)
    {
        if(eventName == "DealDmg") 
        {
            _character.RangeAttackSheelCreate();
        }
        base.EventHandler(eventName);
    }
}
```

**`StateEnemyDead.cs`**
```csharp
public class StateEnemyDead : State<Enemy>
{
    public StateEnemyDead(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._agent.isStopped = true;
        _character._agent.enabled = false;
        _character.OnDead();
    }
}
```