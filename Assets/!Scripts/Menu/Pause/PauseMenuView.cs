using UnityEngine;
using UnityEngine.UI;
using System;

// --- VIEW (MonoBehaviour) ---
public class PauseMenuView : MonoBehaviour
{
    public GameObject _pausePanel;
    public Button _resumeButton;
    public Button _saveButton;
    public Button _loadButton;
    public Button _mainMenuButton;

    public event Action OnResumeClicked;
    public event Action OnSaveClicked;
    public event Action OnLoadClicked;
    public event Action OnMainMenuClicked;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
        _saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
        _loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
    }

    public void ToggleMenu(bool isPaused)
    {
        _pausePanel.SetActive(isPaused);
    }
    
    public void SetLoadButtonInteractable(bool interactable)
    {
        _loadButton.interactable = interactable;
    }
}