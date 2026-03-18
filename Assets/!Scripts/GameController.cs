using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private GameObject _restartCanvas;
    [SerializeField] private GameObject _gameOverTriggerObject; // DIP: Зависим от абстракции

    private IGameOverTrigger _trigger;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        ResumeGame();
    }

    private void Start()
    {
        if (_gameOverTriggerObject != null && _gameOverTriggerObject.TryGetComponent(out _trigger))
        {
            _trigger.OnDeadEvent += GameLose;
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