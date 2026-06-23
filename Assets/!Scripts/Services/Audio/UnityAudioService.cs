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

    private AudioSource _currentMusicSource;

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (_currentMusicSource != null && _currentMusicSource.gameObject != null)
        {
            Object.Destroy(_currentMusicSource.gameObject);
        }

        var go = new GameObject("GlobalMusic");
        _currentMusicSource = go.AddComponent<AudioSource>();
        _currentMusicSource.clip = clip;
        _currentMusicSource.loop = loop;
        _currentMusicSource.Play();
        Object.DontDestroyOnLoad(go);
    }
}