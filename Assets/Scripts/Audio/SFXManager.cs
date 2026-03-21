using UnityEngine;

public class SFXManager : PersistentSingleton<SFXManager>
{
    [SerializeField] private static AudioSource audioSource;
    private static SFXLibrary SFXLibrary;

    protected override void Awake()
    {
        base.Awake();
        if (instance != this) return;
        audioSource = GetComponent<AudioSource>();
        SFXLibrary = GetComponent<SFXLibrary>();
    }

    private void OnEnable()
    {
        if (instance != this) return;
        audioSource = GetComponent<AudioSource>();
        SFXLibrary = GetComponent<SFXLibrary>();
    }

    //to play a sound call "SoundEffectManager.Play("soundname"); on the given object
    public static void Play(string soundName)
    {
        if (instance == null || audioSource == null || SFXLibrary == null) return;
        AudioClip audioClip = SFXLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

    }
}