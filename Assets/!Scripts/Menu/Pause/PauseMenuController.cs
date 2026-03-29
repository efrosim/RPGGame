// --- CONTROLLER (Чистый C# класс) ---

using UnityEngine;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly ISaveService _saveService;
    private readonly ISceneLoaderService _sceneLoader;
    private readonly PlayerController _player; // Ссылка на игрока для сбора данных
    private readonly int _mainMenuIndex;

    private bool _isPaused = false;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, ISceneLoaderService sceneLoader, PlayerController player, int mainMenuIndex)
    {
        _view = view;
        _saveService = saveService;
        _sceneLoader = sceneLoader;
        _player = player;
        _mainMenuIndex = mainMenuIndex;

        _view.OnResumeClicked += TogglePause;
        _view.OnSaveClicked += SaveGame;
        _view.OnLoadClicked += LoadGame;
        _view.OnMainMenuClicked += GoToMainMenu;

        _view.SetLoadButtonInteractable(_saveService.HasSave());
        _view.ToggleMenu(false);
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _view.ToggleMenu(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    private void SaveGame()
    {
        var data = new SaveData
        {
            playerPosX = _player.transform.position.x,
            playerPosY = _player.transform.position.y,
            playerPosZ = _player.transform.position.z,
            playerHealth = _player.HP
        };
        _saveService.SaveGame(data);
        _view.SetLoadButtonInteractable(true);
    }

    private void LoadGame()
    {
        var data = _saveService.LoadGame();
        if (data != null)
        {
            // Отключаем CharacterController/NavMeshAgent если они есть перед телепортом
            _player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            // В идеале добавить метод SetHealth в Character, но для примера:
            // _player.SetHealth(data.playerHealth); 
            TogglePause();
        }
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        _sceneLoader.LoadScene(_mainMenuIndex);
    }
}