using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartbuttonScript : MonoBehaviour
{
    public void ToLvlOne()
    {
        SceneManager.LoadScene("Lvl 1", LoadSceneMode.Single);
    }
}
