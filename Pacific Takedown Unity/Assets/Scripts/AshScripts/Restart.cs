using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void RestartGame(){
        Manager.gameManager._isdead = false;
        SceneManager.LoadScene("AshScene3");
        //Manager.gameManager.displayCanvas.enabled = false;
        
    }

    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
