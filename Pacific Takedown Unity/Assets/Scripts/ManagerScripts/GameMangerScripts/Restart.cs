using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void RestartGame(){
        SceneManager.LoadScene("AshScene3");
        //Manager.gameManager.displayCanvas.enabled = false;
        Manager.gameManager._isdead = false;
    }

    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
