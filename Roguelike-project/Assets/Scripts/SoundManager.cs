using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public AudioSource efxSource;
    public AudioSource musicSource;
    public bool disabled = false;
    public static SoundManager instance = null;


    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    private bool exitPause = false;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.4f);
        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
    }

    //void OnLevelWasLoaded(int index)
    //{
        //if (FindObjectOfTypeEvenIfInactive<SliderMusic>() != null)
            //Debug.Log("trovato");
            //musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
    //}



    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlaySingleShot(AudioClip clip)
    {
        efxSource.clip = clip;
        
        efxSource.PlayOneShot(efxSource.clip, PlayerPrefs.GetFloat("efxVolume", 0.8f) / 2f);
    }

    public void PlaySingleMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }



    public void setVolume(float vol)
    {
        efxSource.volume = vol;
    }
    public void RandomizeSfx(params AudioClip [] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
        //efxSource.volume = 1;
    }

    public void RandomizeSfxShot(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.PlayOneShot(efxSource.clip, PlayerPrefs.GetFloat("efxVolume", 0.8f)/ 2f);
        //efxSource.volume = 1;
    }
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.4f) / 3f;
            exitPause = false;
            
            
        }
        else if(!exitPause && PauseMenu.exitPause)
        {
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.4f);
            exitPause = true;
        }

  


    }


}
