// Добавили <T> и ограничение, что T - это Character или его наследник
public abstract class State<T> : IState where T : Character 
{
    protected T _character; // Теперь поле имеет точный тип (PlayerController, Enemy и т.д.)
    protected StateMachine _SM;

    // Конструктор сразу принимает нужный тип
    public State(T character, StateMachine stateMachine)
    {
        _character = character;
        _SM = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void EventHandler(AnimEnums animstate) { }
    public virtual void LogicUpdate() { }
    public virtual void Update() { }
}