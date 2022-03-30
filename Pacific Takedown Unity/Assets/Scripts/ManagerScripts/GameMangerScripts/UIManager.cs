using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    void Update()
    {
        DontDestroyOnLoad(this.gameObject);    
    }
    /*
    public Canvas displayCanvas;
    public TMP_Text deathText;
    public bool _wasClicked;
    [SerializeField] Button restartButton;

   void Awake()
   {
    restartButton.onClick.AddListener(delegate{ButtonOnClick(true);});

   }

    void Start()
    {
        displayCanvas.enabled = false;
        deathText.text = "You died, idiot!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDeath()
    {
        // if (Manager.instance.GameOver(true))
            Time.timeScale = 0.0f;
            displayCanvas.enabled = true;
    }

    public bool ButtonOnClick(bool flag)
    {
        _wasClicked = flag; //if clicked, then restart the game

        return _wasClicked;
    }
    */
}
