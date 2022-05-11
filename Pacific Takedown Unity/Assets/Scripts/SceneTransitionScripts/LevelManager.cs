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

    [System.Obsolete]
    void Update()
    {
        if (Application.loadedLevelName != "DeathScene")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
    }
}
