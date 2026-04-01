public interface IEnemyStateProvider
{
    string UniqueId { get; }
    EnemyState GetState();
    void RestoreState(EnemyState state);
    void DestroyEntity();
}
