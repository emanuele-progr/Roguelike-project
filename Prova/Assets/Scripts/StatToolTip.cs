using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatToolTip : MonoBehaviour
{
    [SerializeField] Text ObjectNameText;
    [SerializeField] Text StatNameText;
    

    

    public void ShowTooltip(string obejctName, string statName, string statValue)
    {
        ObjectNameText.text = obejctName;
        StatNameText.text = statName + "   +" + statValue;
        


        gameObject.SetActive(true);

    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }


}
