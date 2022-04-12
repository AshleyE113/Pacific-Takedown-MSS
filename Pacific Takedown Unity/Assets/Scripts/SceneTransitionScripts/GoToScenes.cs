using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScenes : MonoBehaviour
{
    public void LevelOne()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
