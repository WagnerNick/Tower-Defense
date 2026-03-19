using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelector;

    private void Awake()
    {
        levelSelector.SetActive(false);
    }

    public void Play()
    {
        levelSelector.SetActive(true);
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
