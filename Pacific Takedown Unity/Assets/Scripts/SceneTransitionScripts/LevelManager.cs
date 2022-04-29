using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneName;
    Scene currentScene;
     
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName != "StartScene" || sceneName != "GOver")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
        
    }
}
