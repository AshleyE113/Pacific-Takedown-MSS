using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAlarmOff : MonoBehaviour
{

    public GameObject alarmButton;

    // Update is called once per frame
    void Update()
    {
        if(alarmButton.GetComponent<ComputerSpriteChange>().changed == true){
            //disable the alarm
            this.gameObject.SetActive(false);
            Debug.Log("CHANGE DETECED");
        }
    }
}
