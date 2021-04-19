using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTrap : MonoBehaviour
{
    public AudioClip trap;
    public bool firstTime = true;
    public Animator animator;

    private string tag;
    private Collider2D coll;



    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if (firstTime)
        {
            firstTime = false;
            if (other.tag == "Player")
            {
                tag = other.tag;
                other.gameObject.GetComponent<Player>().LoseHealth(30);
            }
            else
            {
                tag = "Enemy";
                other.gameObject.GetComponent<Enemy>().LoseHealth(30);
            }

            coll = other;
            Invoke("startTrap", 0.1f);
            
        }

    }

    public void startTrap()
    {
        animator.enabled = true;
        SoundManager.instance.efxSource.PlayOneShot(trap, PlayerPrefs.GetFloat("efxVolume", 0.8f));
        Invoke("Disappear", 0.83f);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
