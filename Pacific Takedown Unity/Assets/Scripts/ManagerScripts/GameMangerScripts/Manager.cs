using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    //private static Manager _instance;
    //public bool _isGameOver;
    public static Manager instance;
    //[SerializeField] UIManager uiManager;
    PlayerController pController;
    public Canvas displayCanvas;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        displayCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void GameOver()
    {
        //Time.timeScale = 0.0f;
        displayCanvas.gameObject.SetActive(true);
        Debug.Log("GOver");
    }
    public void RestartScene()
    {
        //Time.timeScale = 1.0f;
        displayCanvas.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    


    /*public Canvas displayCanvas;
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
             transform.position = player.transform.position; //Follows the player
             if (pController.playerHealth <= 0){
                 _isdead = true;
             }
             else
             {
                 _isdead = false;
             }

             if (_isdead == true)
             {
                 deathText.text = "You died, idiot.";
                 displayCanvas.enabled = true;
             }
             else
             {
                 displayCanvas.enabled = false;
             }
         }
         DontDestroyOnLoad(this.gameObject);
     }*/
}
