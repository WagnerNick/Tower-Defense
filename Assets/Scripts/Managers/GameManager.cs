using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameIsEnded = false;
    public GameObject gameOverUI;
    public GameObject victoryUI;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    public AudioMixer audioMixer;

    float sfxValue;
    float bgmValue;

    private void Awake()
    {
        Instance = this;
        gameOverUI.SetActive(false);
    }

    private void Start()
    {
        audioMixer.GetFloat("SFXvolume", out sfxValue);
        audioMixer.GetFloat("BGMvolume", out bgmValue);
        sfxSlider.value = sfxValue;
        bgmSlider.value = bgmValue;
        MusicManager.Play("GameplayMusic");
        gameIsEnded = false;
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

    public void Retry()
    {
        Time.timeScale = 1.0f;
        SaveManager.Instance?.DeleteSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InputManager.Instance.menuOpen = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        InputManager.Instance.menuOpen = false;
    }

    public void GameOver()
    {
        gameIsEnded = true;
        SaveManager.Instance?.DeleteSave();
        gameOverUI.SetActive(true);
        InputManager.Instance.menuOpen = true;
    }

    public void Victory()
    {
        gameIsEnded = true;
        SaveManager.Instance?.DeleteSave();
        victoryUI.SetActive(true);
        InputManager.Instance.menuOpen = true;
    }
}
