using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class GameplayEntryPoint : MonoBehaviour
{
    [Header("UI Views")]
    [SerializeField] private PauseMenuView _pauseView;
    [SerializeField] private MagicCooldownUI _magicCooldownUI;
    [SerializeField] private GameObject _restartCanvas;
    
    [Header("Scene References")]
    [SerializeField] private PlayerView _player;
    
    [Header("Settings")]
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private PauseMenuController _pauseController;
    public GameController GameController { get; private set; }

    private ISaveInteractor _saveInteractor;
    private ISceneLoaderService _sceneLoader;
    private IAudioService _audioService;

    [Inject]
    public void Construct(ISaveInteractor saveInteractor, ISceneLoaderService sceneLoader, IAudioService audioService)
    {
        _saveInteractor = saveInteractor;
        _sceneLoader = sceneLoader;
        _audioService = audioService;
    }

    private PlayerController _playerController;
    private ScoreboardController _scoreboardController;

    private void Start()
    {
        var playerModel = new PlayerModel(_player._MaxHP, 5f, 2f, 2f);
        _player.Initialize(playerModel);
        _playerController = new PlayerController(playerModel, _player);

        _pauseController = new PauseMenuController(_pauseView, _saveInteractor, _sceneLoader, _mainMenuSceneIndex);
        
        GameController = new GameController(_restartCanvas, _player, _sceneLoader, _audioService, _mainMenuSceneIndex);

        if (_magicCooldownUI != null)
        {
            _magicCooldownUI.Init(playerModel.MagicCooldown);
        }

        var scoreboardView = FindAnyObjectByType<ScoreboardView>();
        if (scoreboardView != null)
        {
            var scoreModel = new ScoreboardModel();
            _scoreboardController = new ScoreboardController(scoreModel, scoreboardView, GameController);
        }

        _pauseAction.action.Enable();
        _pauseAction.action.performed += OnPausePerformed;

        if (_player != null)
        {
            _player.OnDeadEvent += DisablePauseMenu;
        }
    }

    private void Update()
    {
        _playerController?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _playerController?.PhysicsUpdate();
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        _pauseController.TogglePause();
    }

    private void DisablePauseMenu()
    {
        _pauseAction.action.Disable();
    }

    public void RegisterEnemyKill()
    {
        GameController?.RegisterEnemyKill();
    }

    private void OnDestroy()
    {
        _pauseAction.action.performed -= OnPausePerformed;
        
        if (_player != null)
        {
            _player.OnDeadEvent -= DisablePauseMenu;
        }
        
        GameController?.Dispose();
        _playerController?.Dispose();
        _scoreboardController?.Dispose();
    }
}