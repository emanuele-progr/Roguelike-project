using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool help = false;
    public GameObject pauseMenuUI;
    public static bool GameIsPaused = false;
    public static bool exitPause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!help)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        exitPause = true;
    }

    public void Pause()
    {
        exitPause = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        exitPause = true;
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.level = 1;
        GameManager.instance.inventoryIcons.Clear();
        GameManager.instance.player.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
