using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    public GameObject Alarm;
    PlayerController player;

    void Start()
    {
        Alarm.SetActive(true);
    }

    public void TurnOffAlarm()
    {
        Alarm.SetActive(false);
    }

}
