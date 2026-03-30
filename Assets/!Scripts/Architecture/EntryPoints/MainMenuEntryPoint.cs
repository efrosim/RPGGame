using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MainMenuView _view;
    [SerializeField] private int _gameplaySceneIndex = 2;

    private MainMenuController _controller;

    private void Start()
    {
        // Получаем абстракции из локатора
        var audioService = ServiceLocator.Get<IAudioService>();
        var sceneLoader = ServiceLocator.Get<ISceneLoaderService>();

        // Собираем MVC
        _controller = new MainMenuController(_view, audioService, sceneLoader, _gameplaySceneIndex);
    }
}