public class GameStateInteractor : IGameStateInteractor
{
    private readonly IGameStateRepository _repository;
    private readonly IPlayerStateProvider _playerStateProvider;
    private readonly IEnemyRegistry _enemyRegistry;

    public GameStateInteractor(
        IGameStateRepository repository, 
        IPlayerStateProvider playerStateProvider, 
        IEnemyRegistry enemyRegistry)
    {
        _repository = repository;
        _playerStateProvider = playerStateProvider;
        _enemyRegistry = enemyRegistry;
    }

    public bool HasSave() => _repository.HasSave();

    public void SaveGame()
    {
        GameState data = new GameState
        {
            Player = _playerStateProvider.GetState()
        };

        foreach (var enemy in _enemyRegistry.GetAllEnemies())
        {
            data.Enemies.Add(enemy.GetState());
        }

        _repository.Save(data);
    }

    public void LoadGame()
    {
        GameState data = _repository.Load();
        if (data == null) return;

        // Восстанавливаем игрока
        _playerStateProvider.RestoreState(data.Player);

        // Восстанавливаем врагов
        foreach (var enemy in _enemyRegistry.GetAllEnemies())
        {
            var savedEnemy = data.Enemies.Find(e => e.Id == enemy.UniqueId);
            if (savedEnemy.Id != null) // struct check if exists
            {
                enemy.RestoreState(savedEnemy);
            }
            else
            {
                // Если врага нет в сохранении (он был убит до сохранения)
                enemy.DestroyEntity();
            }
        }
    }
}
