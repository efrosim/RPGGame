using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private GameObject _restartCanvas;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        ResumeGame();
    }

    public void GameLose()
    {
        PauseGame();
        _restartCanvas.SetActive(true);
    }

    public void PauseGame() => Time.timeScale = 0.0f;
    
    public void ResumeGame() => Time.timeScale = 1.0f;
}
