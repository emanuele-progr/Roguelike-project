using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour
{

    public Slider slider;
    public Image fill;

    public void SetMaxFoodPoints(int maxFoodPoints)
    {
        slider.maxValue = maxFoodPoints;
        //slider.value = health;
    }
    public void SetFood(int foodPoints)
    {
        slider.value = foodPoints;
    }
}