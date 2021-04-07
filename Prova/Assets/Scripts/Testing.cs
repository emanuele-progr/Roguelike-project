using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Testing : MonoBehaviour
{
    private GridRedefined grid;
    private int width;
    private int lenght;
    private float zoom;
    // Start is called before the first frame update
    private void Start()
    {
        zoom = Camera.main.transform.position.z * (-1f);
        Debug.Log(zoom);
        grid = new GridRedefined(width, lenght, 1f, new Vector3(-1.5f, -.5f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 mousePos = Input.mousePosition;
            grid.SetValue(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zoom)), 56);
            //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zoom)));

        }
    }

    public void setDim(int width, int lenght)
    {
        this.width = width;
        this.lenght = lenght;
       
    }
}
