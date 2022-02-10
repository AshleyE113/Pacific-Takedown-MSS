using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public isDead check_dead;
    public GameObject buttons;

    void Start() {
        buttons.SetActive(false);    
    }
    // Update is called once per frame
    void Update()
    {
        if (check_dead._isdead == true){
            Debug.Log("Is dead ib Manager");
            buttons.SetActive(true);
        }
    }
}
