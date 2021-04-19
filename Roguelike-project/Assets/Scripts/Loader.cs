using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;
 
    

    void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);

        
    }

    public void Restart()
    {
        SceneManager.LoadScene("BoardScene");
        SoundManager.instance.musicSource.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {

        SceneManager.LoadScene("MainMenu");
        GameManager.instance.level = 1;
        GameManager.instance.inventoryIcons.Clear();
        GameManager.instance.player.enabled = false;
    }

}
