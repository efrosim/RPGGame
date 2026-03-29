using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuView : MonoBehaviour
{[Header("Panels")]
    public GameObject _mainPanel;
    public GameObject _settingsPanel;[Header("Buttons")]
    public Button _playButton;
    public Button _settingsButton;
    public Button _closeSettingsButton;

    [Header("Settings")]
    public Slider _volumeSlider;

    // События для контроллера
    public event Action OnPlayClicked;
    public event Action<float> OnVolumeChanged;

    private void Awake()
    {
        _playButton.onClick.AddListener(() => OnPlayClicked?.Invoke());
        _settingsButton.onClick.AddListener(() => ShowSettings(true));
        _closeSettingsButton.onClick.AddListener(() => ShowSettings(false));
        _volumeSlider.onValueChanged.AddListener(val => OnVolumeChanged?.Invoke(val));
    }

    public void ShowSettings(bool show)
    {
        _mainPanel.SetActive(!show);
        _settingsPanel.SetActive(show);
    }

    public void SetVolumeSlider(float value)
    {
        _volumeSlider.value = value;
    }
}