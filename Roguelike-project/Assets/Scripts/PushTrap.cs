using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PushTrap : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioClip trap;

    public bool triggered;
    public bool isRight = false;
    public Animator animator;
    private bool firstTime = true;
    private string tag;

    private Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startTrap()
    {
        animator.enabled = true;
        triggered = true;
        PlaySingle(trap);
        Invoke("Disappear", 0.83f);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (firstTime)
        {
            GameManager.instance.pushTrapUsed += 1;
            firstTime = false;
            if (other.tag == "Player")
            {
                tag = other.tag;
                GameManager.instance.playersTurn = false;
            }
            else
            {
                tag = "Enemy";
            }

            coll = other;
            if(isRight)
                Invoke("Push2", 0.2f);
            else
                Invoke("Push1", 0.2f);
            startTrap();
        }

    }
    public void PlaySingle(AudioClip clip)
    {
        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void Push1()
    {
        if (tag == "Player")
        {
            GameManager.instance.playersTurn = false;
            if (coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -5))
                return;
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -5))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -4);

            }

            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -4))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -3);

            }
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -3))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -2);

            }
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -2))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(0, -1);

            }
        }
        else
        {
            if (coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -5))
                return;
            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -5))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -4);

            }

            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -4))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -3);

            }
            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -3))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -2);

            }

            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -2))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(0, -1);

            }
        }

    }

    public void Push2()
    {
        if (tag == "Player")
        {
            GameManager.instance.playersTurn = false;
            if (coll.gameObject.GetComponent<Player>().MoveFromTrap(5, 0))
                return;
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(5, 0))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(4, 0);

            }

            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(4, 0))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(3, 0);

            }
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(3, 0))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(2, 0);

            }
            if (!coll.gameObject.GetComponent<Player>().MoveFromTrap(2, 0))
            {
                coll.gameObject.GetComponent<Player>().MoveFromTrap(1, 0);

            }
        }
        else
        {
            if (coll.gameObject.GetComponent<Enemy>().MoveFromTrap(5, 0))
                return;
            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(5, 0))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(4, 0);

            }

            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(4, 0))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(3, 0);

            }
            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(3, 0))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(2, 0);

            }
            if (!coll.gameObject.GetComponent<Enemy>().MoveFromTrap(2, 0))
            {
                coll.gameObject.GetComponent<Enemy>().MoveFromTrap(1, 0);

            }
        }

    }
}
