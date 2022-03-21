using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Manager gameManager;
    PlayerController pController;
    GameObject player;
    //public isDead check_dead;
    public Canvas displayCanvas;
    public TMP_Text deathText;
    public bool _isdead = false;

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }
    void Start() {

        player = GameObject.Find("Player");
        if (player != null)
            pController = GameObject.Find("Player").GetComponent<PlayerController>(); //Gets the player controller from Player GO
        //displayCanvas.SetActive(false);
        displayCanvas.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Debug.Log("Exists");
            transform.position = player.transform.position; //Follows the player
            if (pController.playerHealth <= 0){
                _isdead = true;
                deathText.text = "You died, idiot.";
                displayCanvas.enabled = true;
                Debug.Log("is true");
            }
            else
            {
                _isdead = false;
                displayCanvas.enabled = false;
                Debug.Log("Dead ool: " + _isdead);
                Debug.Log(displayCanvas.enabled);
            }
        }
        else
        {
            Debug.Log("GONE!");
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
