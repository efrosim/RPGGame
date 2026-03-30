using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitySceneLoader : ISceneLoaderService
{
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(buildIndex);
    }
}