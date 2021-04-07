using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    public void PlayGame()
    {
        if (GameObject.Find("GameManager(Clone)"))
            GameObject.Find("GameManager(Clone)").GetComponent<GameManager>().level = 0;
        SceneManager.LoadScene("BoardScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
