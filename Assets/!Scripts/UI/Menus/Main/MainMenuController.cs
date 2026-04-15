public class MainMenuController
{
    private readonly MainMenuModel _model;
    private readonly MainMenuView _view;
    private readonly IAudioService _audioService;
    private readonly ISceneLoaderService _sceneLoader;
    private readonly int _gameplaySceneIndex;

    public MainMenuController(MainMenuView view, IAudioService audioService, ISceneLoaderService sceneLoader, int gameplaySceneIndex)
    {
        _view = view;
        _audioService = audioService;
        _sceneLoader = sceneLoader;
        _gameplaySceneIndex = gameplaySceneIndex;
        
        _model = new MainMenuModel { 
            CurrentVolume = _audioService.GetVolume(),
            IsPeacefulMode = GameController.IsPeacefulMode,
        };

        _view.SetVolumeSlider(_model.CurrentVolume);
        _view.SetPeacefulModeToggle(_model.IsPeacefulMode);

        _view.OnPlayClicked += HandlePlay;
        _view.OnVolumeChanged += HandleVolumeChange;
        _view.OnPeacefulModeChange += HandlePeacefulModeChange;
    }

    private void HandlePlay() => _sceneLoader.LoadScene(_gameplaySceneIndex);

    private void HandleVolumeChange(float volume)
    {
        _model.CurrentVolume = volume;
        _audioService.SetVolume(volume);
    }

    private void HandlePeacefulModeChange(bool isPeaceful)
    {
        _model.IsPeacefulMode = isPeaceful;
        GameController.IsPeacefulMode = isPeaceful;
    }
}