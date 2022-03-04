using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    private GameObject parent;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    // Start is called before the first frame update
    public void LaunchAttack()
    {
        parent.GetComponent<EnemyAI>().CommenceAttack();
    }
}
