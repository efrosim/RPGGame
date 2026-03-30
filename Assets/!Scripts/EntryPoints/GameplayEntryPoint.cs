using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayEntryPoint : MonoBehaviour
{
    [Header("UI Views")]
    [SerializeField] private PauseMenuView _pauseView;
    [SerializeField] private MagicCooldownUI _magicCooldownUI;
    [SerializeField] private GameObject _restartCanvas;[Header("Scene References")]
    [SerializeField] private PlayerController _player;
    
    [Header("Settings")]
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private PauseMenuController _pauseController;
    private GameController _gameController;

    private void Start()
    {
        var saveRepository = ServiceLocator.Get<ISaveRepository>();
        var sceneLoader = ServiceLocator.Get<ISceneLoaderService>();

        ISaveInteractor saveInteractor = new SaveInteractor(saveRepository, _player);

        _pauseController = new PauseMenuController(_pauseView, saveInteractor, sceneLoader, _mainMenuSceneIndex);
        
        // Передаем sceneLoader и индекс меню в GameController
        _gameController = new GameController(_restartCanvas, _player, sceneLoader, _mainMenuSceneIndex);

        if (_magicCooldownUI != null)
        {
            _magicCooldownUI.Init(_player.MagicCooldown);
        }

        // Включаем ESC и подписываемся на нажатие
        _pauseAction.action.Enable();
        _pauseAction.action.performed += OnPausePerformed;

        // Подписываемся на смерть игрока, чтобы отключить ESC
        if (_player != null)
        {
            _player.OnDeadEvent += DisablePauseMenu;
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        _pauseController.TogglePause();
    }

    private void DisablePauseMenu()
    {
        _pauseAction.action.Disable();
    }

    private void OnDestroy()
    {
        _pauseAction.action.performed -= OnPausePerformed;
        
        if (_player != null)
        {
            _player.OnDeadEvent -= DisablePauseMenu;
        }
        
        _gameController?.Dispose();
    }
}