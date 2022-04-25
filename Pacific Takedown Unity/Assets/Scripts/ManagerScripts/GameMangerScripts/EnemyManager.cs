using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
 // Use this for initialization
     int enemiesLeft = 0;
     bool killedAllEnemies = false;
    public GameObject nextSceneLoader;
    GameObject[] enemies;
     void Start () {
        enemies = GameObject.FindGameObjectsWithTag("enemy");

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
        nextSceneLoader.SetActive(true);

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

