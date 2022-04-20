using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Animator[] healthMeters;

    private void Start()
    {
        for (int i = 0; i < healthMeters.Length; i++)
        {
            healthMeters[i].enabled = false;
        }
    }

    public void SetMaxHealth(int health)
    {
    }

    public void SetHealth(int health)
    {
        healthMeters[health].enabled = true;
    }
    
}
