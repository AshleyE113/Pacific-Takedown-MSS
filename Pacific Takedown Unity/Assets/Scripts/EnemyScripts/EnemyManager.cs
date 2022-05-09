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
    private bool soundPlayed;
    GameObject pointer;

    void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        killedAllEnemies = false;
        //pointer = GameObject.FindGameObjectWithTag("PointerGO");
        //pointer.SetActive(false);
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
        //pointer.SetActive(true);
        if(!soundPlayed)
        {
            AkSoundEngine.PostEvent("Play_LevelClear" , gameObject);
            soundPlayed = true;
        }
        Debug.Log("Pointing");

    }
     
}

