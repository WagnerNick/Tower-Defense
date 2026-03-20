using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelector;
    [SerializeField] private GameObject credits;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    public AudioMixer audioMixer;

    float sfxValue;
    float bgmValue;

    private void Awake()
    {
        levelSelector.SetActive(false);
        credits.SetActive(false);
    }

    private void Start()
    {
        audioMixer.GetFloat("SFXvolume", out sfxValue);
        sfxSlider.value = sfxValue;
        audioMixer.GetFloat("BGMvolume", out bgmValue);
        bgmSlider.value = bgmValue;
        MusicManager.Play("MenuMusic");
        InputManager.Instance.OnMenu += HandleMenu;
    }

    public void SfxVolume(float volume)
    {
        if (volume == -25)
        {
            audioMixer.SetFloat("SFXvolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXvolume", volume);
        }
    }

    public void BgmVolume(float volume)
    {
        if (volume == -35)
        {
            audioMixer.SetFloat("BGMvolume", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMvolume", volume);
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnMenu -= HandleMenu;
    }

    void HandleMenu()
    {
        if (PlacementSystem.Instance != null && PlacementSystem.Instance.isActive)
            return;
        if (credits.activeSelf)
            ToggleCredits();
        if (levelSelector.activeSelf)
            TogglePlay();
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
        InputManager.Instance.menuOpen = credits.activeSelf ? true : false;
    }

    public void TogglePlay()
    {
        levelSelector.SetActive(!levelSelector.activeSelf);
        InputManager.Instance.menuOpen = levelSelector.activeSelf ? true : false;
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {

        Application.Quit();
    }
}
