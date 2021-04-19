using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //Initialization code goes here[/INDENT]
        }
        else
        {
            Destroy(gameObject);
        }
    }
}