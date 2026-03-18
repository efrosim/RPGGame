using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance; // Оставим этот синглтон, если он нужен для UI кнопок рестарта

    [SerializeField] private GameObject _restartCanvas;
    
    // ИСПРАВЛЕНО: Добавили ссылку на игрока для подписки на его смерть
    [SerializeField] private Character _player; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        ResumeGame();
    }

    private void Start()
    {
        // ИСПРАВЛЕНО: Подписываемся на событие смерти через нормальную ссылку
        if (_player != null)
        {
            _player.OnDeadEvent += GameLose;
        }
        else
        {
            Debug.LogWarning("Игрок не назначен в GameController!");
        }
    }

    public void GameLose()
    {
        PauseGame();
        _restartCanvas.SetActive(true);
    }

    public void PauseGame() => Time.timeScale = 0.0f;
    public void ResumeGame() => Time.timeScale = 1.0f;
}