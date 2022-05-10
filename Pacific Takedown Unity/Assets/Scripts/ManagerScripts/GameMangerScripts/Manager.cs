using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    bool spawned = false;

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
        //Sound
        AkSoundEngine.PostEvent("Play_Music" , gameObject);
    }

    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GOver", LoadSceneMode.Single);
    }

    public void RestartScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
