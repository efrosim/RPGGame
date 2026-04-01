using UnityEngine;

public class ProjectEntryPoint : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        IAudioService audioService = new UnityAudioService();
        IRepository saveRepository = new JsonGameStateRepository(); 
        ISceneLoaderService sceneLoader = new UnitySceneLoader();

        ServiceLocator.Register(audioService);
        ServiceLocator.Register(saveRepository);
        ServiceLocator.Register(sceneLoader);

        audioService.SetVolume(audioService.GetVolume());
        sceneLoader.LoadScene(_mainMenuSceneIndex);
    }
}