using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    // Use this for initialization
    int enemiesLeft = 0;
    public static bool killedAllEnemies = false;
    public GameObject[] enemies;
    void Start () {
        enemiesLeft = 10; // or whatever;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
     
    // Update is called once per frame
    void Update () {

        bool Gameover = true;
        int totalEnemiesAlive = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                Gameover = false;
                totalEnemiesAlive += 1;
            }
        }

        if (Gameover) endGame();
    }

    void endGame()
    {
        killedAllEnemies = true;
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

