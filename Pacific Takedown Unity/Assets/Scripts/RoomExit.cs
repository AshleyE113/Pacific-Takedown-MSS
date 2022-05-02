using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    [SerializeField] GameObject LDoorHalf, RDoorHalf, player;
    Vector3 LDoorPos, RDoorPos;

    void Start()
    {
        player = GameObject.Find("Player");
        LDoorPos = LDoorHalf.transform.position;
        RDoorPos = RDoorHalf.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyManager.killedAllEnemies == true && Vector2.Distance(gameObject.transform.position, player.transform.position) < 8)
        {
            if (LDoorPos.x > -0.56f)
                LDoorHalf.transform.position = Vector3.Lerp(LDoorHalf.transform.position, LDoorHalf.transform.position + new Vector3(-1f, 0, 0), 0.7f * Time.deltaTime);
            if (RDoorPos.x > 0.66f)
                RDoorHalf.transform.position = Vector3.Lerp(RDoorHalf.transform.position, RDoorHalf.transform.position + new Vector3(1f, 0, 0), 0.7f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (EnemyManager.killedAllEnemies == true)
            {
                GameObject.FindWithTag("LevelTransition").GetComponent<LevelTransition>().Transition();
            }
        }
    }
}
