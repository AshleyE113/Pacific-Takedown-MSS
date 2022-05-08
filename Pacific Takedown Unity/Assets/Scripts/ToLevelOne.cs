using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ToLevelOne : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToLvlOne();
        }
    }

    public void ToLvlOne()
    {
        SceneManager.LoadScene("Lvl 1", LoadSceneMode.Single);
    }
}
