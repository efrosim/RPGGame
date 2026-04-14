using UnityEngine;
using System.Threading.Tasks;

public class GameController
{
    private readonly GameObject _restartCanvas;
    private readonly IGameOverTrigger _trigger;
    private readonly ISceneLoaderService _sceneLoader;
    private readonly IAudioService _audioService;
    private readonly AudioClip _victoryMusic;
    private readonly int _mainMenuIndex;
    
    private bool _isGameOver = false;

    public static bool IsPeacefulMode = false;
    public int EnemyKillCount { get; private set; }

    public event System.Action OnBossSpawnRequested;
    public event System.Action<int> OnKillCountChanged;

    public GameController(GameObject restartCanvas, IGameOverTrigger trigger, ISceneLoaderService sceneLoader, IAudioService audioService, int mainMenuIndex)
    {
        _restartCanvas = restartCanvas;
        _trigger = trigger;
        _sceneLoader = sceneLoader;
        _audioService = audioService;
        _mainMenuIndex = mainMenuIndex;

        // Load victory music from resources
        _victoryMusic = Resources.Load<AudioClip>("VictoryMusic"); // Assume we have one

        if (_trigger != null)
        {
            _trigger.OnDeadEvent += GameLose;
        }

        ResumeGame();
    }
    
    public void RegisterEnemyKill()
    {
        if (_isGameOver) return;

        EnemyKillCount++;
        OnKillCountChanged?.Invoke(EnemyKillCount);
        
        if (EnemyKillCount == 3)
        {
            OnBossSpawnRequested?.Invoke();
        }
        else if (EnemyKillCount == 5)
        {
            GameWin();
        }
    }

    private void GameWin()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        if (_victoryMusic != null)
            _audioService.PlayMusic(_victoryMusic);

        // Transition back to main menu after some time
        _ = WinSequenceAsync();
    }

    private async Task WinSequenceAsync()
    {
        await Task.Delay(3000);
        _sceneLoader.LoadScene(_mainMenuIndex);
    }

    public async void GameLose()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        PauseGame();
        _restartCanvas.SetActive(true);

        await Task.Delay(5000);
        _sceneLoader.LoadScene(_mainMenuIndex);
    }

    public void PauseGame() 
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Dispose()
    {
        if (_trigger != null)
        {
            _trigger.OnDeadEvent -= GameLose;
        }
    }
}