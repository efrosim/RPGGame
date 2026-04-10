using VContainer;
using VContainer.Unity;
using UnityEngine;

public class Bootstrapper : IStartable
{
    private readonly IAudioService _audioService;
    private readonly ISceneLoaderService _sceneLoader;
    private readonly int _mainMenuSceneIndex; // Убрали жесткую привязку "= 1"

    // Добавили int mainMenuSceneIndex в параметры
    public Bootstrapper(IAudioService audioService, ISceneLoaderService sceneLoader, int mainMenuSceneIndex)
    {
        _audioService = audioService;
        _sceneLoader = sceneLoader;
        _mainMenuSceneIndex = mainMenuSceneIndex;
    }

    public void Start()
    {
        // First scene load setup
        _audioService.SetVolume(_audioService.GetVolume());
        _sceneLoader.LoadScene(_mainMenuSceneIndex);
    }
}