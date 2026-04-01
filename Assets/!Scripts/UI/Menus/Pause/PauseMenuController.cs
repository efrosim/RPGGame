using UnityEngine;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly IGameStateInteractor _saveInteractor; 
    private readonly ISceneLoaderService _sceneLoader;
    private readonly int _mainMenuIndex;

    private bool _isPaused = false;

    public PauseMenuController(PauseMenuView view, IGameStateInteractor saveInteractor, ISceneLoaderService sceneLoader, int mainMenuIndex)
    {
        _view = view;
        _saveInteractor = saveInteractor;
        _sceneLoader = sceneLoader;
        _mainMenuIndex = mainMenuIndex;

        _view.OnResumeClicked += TogglePause;
        _view.OnSaveClicked += SaveGame;
        _view.OnLoadClicked += LoadGame;
        _view.OnMainMenuClicked += GoToMainMenu;

        _view.SetLoadButtonInteractable(_saveInteractor.HasSave());
        _view.ToggleMenu(false);
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _view.ToggleMenu(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
        
        Cursor.visible = _isPaused;
        Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void SaveGame()
    {
        _saveInteractor.SaveGame();
        _view.SetLoadButtonInteractable(true);
    }

    private void LoadGame()
    {
        _saveInteractor.LoadGame();
        TogglePause();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        _sceneLoader.LoadScene(_mainMenuIndex);
    }
}