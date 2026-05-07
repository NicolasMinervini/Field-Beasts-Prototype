using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject pauseMenu, winMenu, gameHUD;

    public bool canPauseWithEscape = true;

    public string mainMenuSceneName = "MainMenu";

    public TurnManager tm;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if(canPauseWithEscape && pauseMenu != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(pauseMenu.activeSelf == false && (winMenu == null || winMenu.activeSelf == false))
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        if(gameHUD != null)
        {
            gameHUD.SetActive(false);
        }

        if (tm != null) tm.PauseDeselect();
    }

    public void GameEnd()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        if (gameHUD != null)
        {
            gameHUD.SetActive(false);
        }
        winMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        if (gameHUD != null && (winMenu == null || winMenu.activeSelf == false))
        {
            gameHUD.SetActive(true);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
