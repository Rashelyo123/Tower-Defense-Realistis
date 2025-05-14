using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentRound = 0;
    public int totalEnemies = 0;
    public int PlayerMoney = 100;
    public bool gameActive = true;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartRound();
    }
    public void StartRound()
    {
        currentRound++;
        totalEnemies = currentRound * 5;
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject enemy = Instantiate(Resources.Load("EnemyPrefab") as GameObject);
            enemy.transform.position = new Vector3(3, 97, i * 2);

        }
    }
    public void EndGame()
    {
        gameActive = false;
        Debug.Log("Game Over! You reached round " + currentRound);
    }
}
