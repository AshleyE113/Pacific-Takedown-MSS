using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScenes : MonoBehaviour
{
    [SerializeField] GameObject DeathCanvas;
    bool _spawned;

    private void Start()
    {
        _spawned = false;
    }
    public void LevelOne()
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        Debug.Log("Loaded");
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GOver" && _spawned == false)
        {
            Instantiate(DeathCanvas);
            _spawned = true;
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        Debug.Log("Back in Lvl1");
        Destroy(DeathCanvas);
    }
}
