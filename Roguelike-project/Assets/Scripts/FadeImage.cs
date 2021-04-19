using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image img;
    public bool bianco = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeIn()
    {
        StartCoroutine(ImageFade(false));
    }

    public void FadeOut()
    {
        StartCoroutine(ImageFade(true));
    }

    IEnumerator ImageFade(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                if(bianco)
                    img.color = new Color(1, 1, 1, i);
                else
                    img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                if (bianco)
                    img.color = new Color(1, 1, 1, i);
                else
                    img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
