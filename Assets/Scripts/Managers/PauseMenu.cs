using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public SpeedUpManager speedUpManager;

    private void Start()
    {
        InputManager.Instance.OnMenu += HandleMenu;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnMenu -= HandleMenu;
    }

    void HandleMenu()
    {
        if (PlacementSystem.Instance != null && PlacementSystem.Instance.isActive)
            return;
        if (InputManager.Instance != null && InputManager.Instance.menuOpen)
            return;
        Toggle();
    }

    public void Toggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (speedUpManager != null && speedUpManager.spedUp)
            Time.timeScale = pauseMenu.activeSelf ? 0f : speedUpManager.speed;
        else
            Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
    }
}
