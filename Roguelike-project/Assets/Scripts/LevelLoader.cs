using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    // Start is called before the first frame update
    
    public Slider musicSlider;
    public Slider efxSlider;
    private bool disabled = false;
    void Start()
    {
        SoundManager.instance.musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.4f);
        if (musicSlider != null)
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.4f);

        if (efxSlider != null)
            efxSlider.value = PlayerPrefs.GetFloat("efxVolume", 0.8f);
    }
    public void setDisabled()
    {

        if (disabled)
        {
            disabled = false;
            SoundManager.instance.musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.4f);
        }
        else
        {
            disabled = true;
            SoundManager.instance.musicSource.volume = 0f;
        }

    }

    public void LoadNextScene(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void sliderSetVolume()
    {
        SoundManager.instance.musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", SoundManager.instance.musicSource.volume);
    }

    public void sliderSetEfxVolume()
    {
        SoundManager.instance.efxSource.volume = efxSlider.value;
        PlayerPrefs.SetFloat("efxVolume", SoundManager.instance.efxSource.volume);
    }

    public void HelpEvent()
    {
        
        AnalyticsResult result = Analytics.CustomEvent("helpActivated");
        Debug.Log("result" + result);
    }

    public void DisableDialogEvent()
    {

        AnalyticsResult result = Analytics.CustomEvent("disableDialogActivated");
        Debug.Log("result" + result);
    }
}
