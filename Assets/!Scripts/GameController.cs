using UnityEngine;
using System.Threading.Tasks;

public class GameController
{
    private readonly GameObject _restartCanvas;
    private readonly IGameOverTrigger _trigger;
    private readonly ISceneLoaderService _sceneLoader;
    private readonly int _mainMenuIndex;
    
    private bool _isGameOver = false;

    // Добавили ISceneLoaderService и mainMenuIndex в конструктор
    public GameController(GameObject restartCanvas, IGameOverTrigger trigger, ISceneLoaderService sceneLoader, int mainMenuIndex)
    {
        _restartCanvas = restartCanvas;
        _trigger = trigger;
        _sceneLoader = sceneLoader;
        _mainMenuIndex = mainMenuIndex;

        if (_trigger != null)
        {
            _trigger.OnDeadEvent += GameLose;
        }

        ResumeGame();
    }
    
    public async void GameLose()
    {
        if (_isGameOver) return; // Защита от двойного срабатывания
        _isGameOver = true;

        PauseGame();
        _restartCanvas.SetActive(true);

        // Ждем 5000 миллисекунд (5 секунд).
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