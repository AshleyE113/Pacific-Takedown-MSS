using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene : MonoBehaviour
{
    LevelManager lvlManager;
    void Start()
    {
        lvlManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(lvlManager.sceneName, LoadSceneMode.Single);
        Debug.Log(lvlManager.sceneName);
    }
}
