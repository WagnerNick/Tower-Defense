using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

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
        Toggle();
    }

    public void Toggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
    }
}
