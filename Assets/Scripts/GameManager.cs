using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameIsEnded = false;

    public GameObject endScreenUI;
    public GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
        endScreenUI.SetActive(false);
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

    public void Menu()
    {

    }

    private void EndGame()
    {
        gameIsEnded = true;
        endScreenUI.SetActive(true);
    }

    public void GameOver()
    {
        EndGame();
        gameOverUI.SetActive(true);
    }
}
