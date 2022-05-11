using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScenes : MonoBehaviour
{
    [SerializeField] GameObject DeathCanvas;
    bool _spawned;
    LevelManager lvlManager;

    private void Start()
    {
        lvlManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _spawned = false;
    }
    public void LevelOne()
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        
    }
    /*
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GOver" && _spawned == false)
        {
            Instantiate(DeathCanvas);
            _spawned = true;
        }
    }*/
    public void RestartScene()
    {
        SceneManager.LoadScene(lvlManager.sceneName, LoadSceneMode.Single);
        Debug.Log(lvlManager.sceneName);
    }
}
