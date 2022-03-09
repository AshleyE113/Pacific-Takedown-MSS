using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController pController;
    GameObject player;
    //public isDead check_dead;
    public Canvas displayCanvas;
    public TMP_Text deathText;
    public bool _isdead;

    void Start() {

        player = GameObject.Find("Player");
        if (player != null)
            pController = GameObject.Find("Player").GetComponent<PlayerController>(); //Gets the player controller from Player GO
        displayCanvas.enabled = false;    
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position; //Follows the player
            if (pController.playerHealth <= 0){
                deathText.text = "You died, idiot.";
                displayCanvas.enabled = true;
            }
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}
