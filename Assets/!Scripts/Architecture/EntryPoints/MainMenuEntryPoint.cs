using UnityEngine;
using VContainer;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MainMenuView _view;
    [SerializeField] private int _gameplaySceneIndex = 2;

    private MainMenuController _controller;
    private IAudioService _audioService;
    private ISceneLoaderService _sceneLoader;

    [Inject]
    public void Construct(IAudioService audioService, ISceneLoaderService sceneLoader)
    {
        _audioService = audioService;
        _sceneLoader = sceneLoader;
    }

    private void Start()
    {
        _controller = new MainMenuController(_view, _audioService, _sceneLoader, _gameplaySceneIndex);
    }
}