using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    EnemyAI enemyScript;
    [SerializeField] ComputerSpriteChange alarm;
    [SerializeField] GameObject alarmGO;
    void Start()
    {
        enemyScript = GetComponent<EnemyAI>();
        alarm = alarm.GetComponent<ComputerSpriteChange>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(enemyScript.rb.position, enemyScript.target.position) < enemyScript.attackRange)
        {
            if (alarm.changed == false)
                alarmGO.SetActive(true);
            else
                alarmGO.SetActive(false);
        }
       
    }
}
