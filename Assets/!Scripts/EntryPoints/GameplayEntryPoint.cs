using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseView;
    [SerializeField] private PlayerController _player;
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private PauseMenuController _pauseController;

    private void Start()
    {
        // Получаем сервисы
        var saveService = ServiceLocator.Get<ISaveService>();
        var sceneLoader = ServiceLocator.Get<ISceneLoaderService>();

        // Собираем MVC паузы
        _pauseController = new PauseMenuController(_pauseView, saveService, sceneLoader, _player, _mainMenuSceneIndex);

        // Настраиваем инпут для паузы
        _pauseAction.action.Enable();
        _pauseAction.action.performed += ctx => _pauseController.TogglePause();
    }

    private void OnDestroy()
    {
        _pauseAction.action.performed -= ctx => _pauseController.TogglePause();
    }
}