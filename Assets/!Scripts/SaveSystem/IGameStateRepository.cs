public interface IGameStateRepository
{
    void Save(GameState data);
    GameState Load();
    bool HasSave();
}
