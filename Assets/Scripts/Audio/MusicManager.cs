using UnityEngine;

public class MusicManager : PersistentSingleton<MusicManager>
{
    private static AudioSource audioSource;
    private static MusicLibrary musicLibrary;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicLibrary = GetComponent<MusicLibrary>();
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = musicLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public static void Stop()
    {
        audioSource.Stop();
    }
}
