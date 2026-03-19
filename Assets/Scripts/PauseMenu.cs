using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    void Update()
    {
        if (PlacementSystem.Instance != null)
        { if (PlacementSystem.Instance.isActive) return; }
        if (InputManager.MenuWasPressed)
            Toggle();

    }

    public void Toggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
