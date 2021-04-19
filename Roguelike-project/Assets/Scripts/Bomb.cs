using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool isGift = false;
    public bool triggered = false;
    public bool explosion = false;
    public AudioSource efxSource;
    
    public Animator animator;
    public AudioClip clip;

    private string name;
    private Collider2D trigger;
    private bool coroutineCalled = false;


    public void Update()
    {
        if(triggered && !explosion)
        {
            if (!coroutineCalled)
            {
                if (name == "Player2(Clone)")
                    StartCoroutine("colorGreen");
                else
                    StartCoroutine("colorRed");
            }
        }
            
    }
    public void startBomb()
    {
        if(!isGift)
            animator.enabled = true;
        triggered = true;
    }

    public void explodeBomb()
    {
        explosion = true;
        if(isGift)
            animator.enabled = true;
        else
            animator.SetTrigger("Explode");
        PlaySingle(clip);
        //enabled = false;
        Invoke("Disappear", 1.0f);
        
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered)
        {
            trigger = other;
            name = other.gameObject.name;
            startBomb();
        }
        else if(other.gameObject.name != name)
        {
            if (other.tag == "Player")
            {
                
                other.gameObject.GetComponent<Player>().LoseFood(20);
                other.gameObject.GetComponent<Player>().stunCounter = 8;
                explodeBomb();
            }
            else if (other.tag == "Enemy" && trigger.tag != "Enemy"){

                
                other.gameObject.GetComponent<Enemy>().LoseFood(20);
                other.gameObject.GetComponent<Enemy>().stunCounter = 8;
                explodeBomb();
            }

            
        }
        else if (explosion)
        {
            /*if(FindRadiusDamage("Player").GetComponent<Player>() != null)
            {
                if (!FindRadiusDamage("Player").GetComponent<Player>().stunned)
                {
                    FindRadiusDamage("Player").GetComponent<Player>().LoseFood(20);
                    FindRadiusDamage("Player").GetComponent<Player>().stunCounter = 8;
                }
            }*/
            if (FindRadiusDamage("Enemy").GetComponent<Enemy>() != null)
            {
                if (FindRadiusDamage("Enemy").name != name)
                {
                    if (!FindRadiusDamage("Enemy").GetComponent<Enemy>().stunned)
                    {
                        FindRadiusDamage("Enemy").GetComponent<Enemy>().LoseFood(20);
                        FindRadiusDamage("Enemy").GetComponent<Enemy>().stunCounter = 8;
                    }
                }

            }

        }

    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
        efxSource.clip = clip;
        efxSource.PlayOneShot(efxSource.clip, efxSource.volume);
    }

    public GameObject FindRadiusDamage(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = 4;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (!go.GetComponent<Enemy>().stunned)
            {
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    IEnumerator colorRed()
    {
        coroutineCalled = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.3f);

        coroutineCalled = false;
    }

    IEnumerator colorGreen()
    {
        coroutineCalled = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.3f);

        coroutineCalled = false;
    }
}
