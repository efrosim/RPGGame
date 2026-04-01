using UnityEngine;

public class ProjectEntryPoint : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        IAudioService audioService = new UnityAudioService();
        ISaveRepository saveRepository = new JsonSaveRepository(); 
        ISceneLoaderService sceneLoader = new UnitySceneLoader();
        
        SaveableRegistry saveableRegistry = new SaveableRegistry();
        ServiceLocator.Register(saveableRegistry);
        
        ServiceLocator.Register(audioService);
        ServiceLocator.Register(saveRepository);
        ServiceLocator.Register(sceneLoader);

        audioService.SetVolume(audioService.GetVolume());
        sceneLoader.LoadScene(_mainMenuSceneIndex);
    }
}