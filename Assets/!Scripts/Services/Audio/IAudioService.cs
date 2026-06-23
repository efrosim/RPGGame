public interface IAudioService
{
    void SetVolume(float volume);
    float GetVolume();
    void PlayMusic(UnityEngine.AudioClip clip, bool loop = true);
}