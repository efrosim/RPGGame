using UnityEngine;

public class ProjectEntryPoint : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Инициализация конкретных сервисов (Top-Down)
        IAudioService audioService = new UnityAudioService();
        ISaveService saveService = new JsonSaveService();
        ISceneLoaderService sceneLoader = new UnitySceneLoader();

        // Регистрация в локаторе
        ServiceLocator.Register(audioService);
        ServiceLocator.Register(saveService);
        ServiceLocator.Register(sceneLoader);

        // Применяем сохраненную громкость
        audioService.SetVolume(audioService.GetVolume());

        // Загружаем главное меню
        sceneLoader.LoadScene(_mainMenuSceneIndex);
    }
}