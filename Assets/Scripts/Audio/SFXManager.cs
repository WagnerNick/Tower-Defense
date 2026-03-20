using UnityEngine;

public class SFXManager : PersistentSingleton<SFXManager>
{
    [SerializeField] private static AudioSource audioSource;
    private static SFXLibrary soundEffectLibrary;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundEffectLibrary = GetComponent<SFXLibrary>();
    }

    //to play a sound call "SoundEffectManager.Play("soundname"); on the given object
    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}