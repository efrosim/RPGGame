public abstract class StatusEffect
{
    public float Duration { get; }
    public float ElapsedTime { get; private set; }
    public bool IsFinished => ElapsedTime >= Duration;

    protected StatusEffect(float duration)
    {
        Duration = duration;
        ElapsedTime = 0f;
    }

    public void ResetDuration()
    {
        ElapsedTime = 0f;
    }

    public virtual void OnApply(PlayerModel model) { }

    public void Tick(PlayerModel model, float deltaTime)
    {
        ElapsedTime += deltaTime;
        OnTick(model, deltaTime);
    }

    protected virtual void OnTick(PlayerModel model, float deltaTime) { }
    public virtual void OnRemove(PlayerModel model) { }
}
