public interface IPlayerStateProvider
{
    PlayerState GetState();
    void RestoreState(PlayerState state);
}
