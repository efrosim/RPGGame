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
        
        _model = new MainMenuModel { CurrentVolume = _audioService.GetVolume() };
        _view.SetVolumeSlider(_model.CurrentVolume);

        // Подписка на события View
        _view.OnPlayClicked += HandlePlay;
        _view.OnVolumeChanged += HandleVolumeChange;
    }

    private void HandlePlay() => _sceneLoader.LoadScene(_gameplaySceneIndex);

    private void HandleVolumeChange(float volume)
    {
        _model.CurrentVolume = volume;
        _audioService.SetVolume(volume);
    }
}