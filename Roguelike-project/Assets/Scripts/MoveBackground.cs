using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBackground : MonoBehaviour
{
    public Image background;
    public AudioClip musicMenu;
    private bool up = false;
    private bool down = false;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySingleMusic(musicMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 end1 = new Vector3(469.5f, 0f, 0f);
        
        if (background.gameObject.transform.position.y >= 410.4f)
        {
            up = true;
            down = false;
        }
        
        if (background.gameObject.transform.position.y <= 0.4f)
        {
            up = false;
            down = true;
        }
        if (up)
        {
           
            background.gameObject.transform.position += new Vector3(0f, -25f * Time.deltaTime, 0f);
        }
        if (down)
        {
            
            background.gameObject.transform.position += new Vector3(0f, 25f * Time.deltaTime, 0f);
        }

        //StartCoroutine(MoveOverSeconds(background.gameObject, new Vector3(469.5f, -217f, 0f), 5f));
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = end;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;



    }
}

