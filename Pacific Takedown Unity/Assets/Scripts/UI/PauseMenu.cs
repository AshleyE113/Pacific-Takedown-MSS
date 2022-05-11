using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuGO;

    [System.Obsolete]
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape) && Application.loadedLevelName != "DeathScene"  && Application.loadedLevelName != "To Be Continued")
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        } 
    }
    public void Pause()
    {
        Debug.Log("PAUSED");
        PauseMenuGO.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }
    public void Resume()
    {
        Debug.Log("RESUME");
        PauseMenuGO.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
