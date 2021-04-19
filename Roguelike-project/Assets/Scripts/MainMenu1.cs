using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    public bool triggerDialog = true;
    
    public AudioClip tik;
    public Toggle toggle;
    

    public void PlayGame()
    {
        SoundManager.instance.PlaySingle(tik);
        if (GameObject.Find("GameManager(Clone)"))
        {
            GameObject.Find("GameManager(Clone)").GetComponent<GameManager>().level = 0;
            GameObject.Find("GameManager(Clone)").GetComponent<GameManager>().fromMenu = true;
            GameManager.instance.firstTurn = true;
        }
        GameObject.Find("Background").GetComponent<FadeImage>().FadeOut();
        Invoke("Load", 0.4f);
    }

    public void Load()
    {
        SceneManager.LoadScene("BoardScene");
    }

    public void Start()
    {
        if (PlayerPrefs.GetInt("dialog", 1) == 1)
        {
            
            triggerDialog = true;
            if( toggle != null)
                toggle.isOn = true;
        }
        else
        {
            triggerDialog = false;
            if(toggle != null)
                toggle.isOn = false;
        }
        if (GameObject.Find("DialogueManager"))
            GameObject.Find("DialogueManager").GetComponent<DialogueManager>().dialogOn = triggerDialog;
    }

    public void QuitGame()
    {
        SoundManager.instance.PlaySingle(tik);
        Invoke("Quit", 0.2f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayTik()
    {
        SoundManager.instance.PlaySingle(tik);
    }

    public void setDialog()
    {

        if (DialogueManager.instance.dialogOn) {
            GameManager.instance.dialogOn = false;
            DialogueManager.instance.dialogOn = false;
                }


        else
            //DialogueManager.instance.dialogOn = true;

            Debug.Log(DialogueManager.instance.dialogOn);
    }

    public void setDialogSettings()
    {
        
        if (toggle.isOn)
            PlayerPrefs.SetInt("dialog", 1);
        else
            PlayerPrefs.SetInt("dialog", 0);
    }

}
