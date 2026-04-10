using UnityEngine;

public class UnityAudioService : IAudioService
{
    private const string VolumeKey = "GameVolume";

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() => PlayerPrefs.GetFloat(VolumeKey, 1f);

    public void PlayMusic(AudioClip clip)
    {
        var go = new GameObject("GlobalMusic");
        var source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.Play();
        Object.DontDestroyOnLoad(go);
    }
}