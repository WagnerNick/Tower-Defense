using UnityEngine;

public class MusicManager : PersistentSingleton<MusicManager>
{
    private static AudioSource audioSource;
    private static MusicLibrary musicLibrary;

    protected override void Awake()
    {
        base.Awake();
        if (instance != this) return;
        audioSource = GetComponent<AudioSource>();
        musicLibrary = GetComponent<MusicLibrary>();
    }

    private void OnEnable()
    {
        if (instance != this) return;
        audioSource = GetComponent<AudioSource>();
        musicLibrary = GetComponent<MusicLibrary>();
    }

    public static void Play(string soundName)
    {
        if (instance == null || audioSource == null || musicLibrary == null) return;
        AudioClip audioClip = musicLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public static void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
