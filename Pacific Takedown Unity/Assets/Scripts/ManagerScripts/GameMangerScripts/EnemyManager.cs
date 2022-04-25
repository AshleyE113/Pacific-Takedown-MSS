using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
 // Use this for initialization
    int enemiesLeft = 0;
    bool killedAllEnemies = false;
    GameObject[] enemies;
     void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemiesLeft = enemies.Length;// or whatever;

    }
     
     // Update is called once per frame
     void Update () {

         if(enemiesLeft == 0)
         {
            GoToNextLevel();
         }
     }
     
     void GoToNextLevel()
     {
         killedAllEnemies = true;
        SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
        Debug.Log("Time for level 2!");

     }
     
     void OnGUI()
     {
         if(killedAllEnemies)
         {
         GUI.Label(new Rect (0,0,200,20),"all gone");
         }
         else
         {
             GUI.Label(new Rect (0,0,200,20),"Enemies Remaining : " + enemiesLeft);
         }
     }
}

