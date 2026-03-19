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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {

    }

    public void GameOver()
    {
        gameIsEnded = true;
        gameOverUI.SetActive(true);
    }
}
