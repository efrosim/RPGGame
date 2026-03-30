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
        // Получаем Репозиторий из локатора
        var saveRepository = ServiceLocator.Get<ISaveRepository>();
        var sceneLoader = ServiceLocator.Get<ISceneLoaderService>();

        // Создаем Интерактор (Бизнес-логика)
        ISaveInteractor saveInteractor = new SaveInteractor(saveRepository, _player);

        // Собираем MVC паузы, передавая Интерактор
        _pauseController = new PauseMenuController(_pauseView, saveInteractor, sceneLoader, _mainMenuSceneIndex);

        _pauseAction.action.Enable();
        _pauseAction.action.performed += ctx => _pauseController.TogglePause();
    }

    private void OnDestroy()
    {
        _pauseAction.action.performed -= ctx => _pauseController.TogglePause();
    }
}