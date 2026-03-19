using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameIsEnded = false;

    public GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
        gameOverUI.SetActive(false);
    }

    private void Start()
    {
        gameIsEnded = false;
    }

    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        gameIsEnded = true;
        gameOverUI.SetActive(true);
    }

    public void Victory()
    {
        Debug.Log("You won");
    }
}
