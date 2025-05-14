// AliyerEdon@mail.com Christmas 2022
// Wave based spawner
// Use this component to spawn enemies based on the limited count value / option in the random selected points

//*** Use first point of each waypoints group as spawn points

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Space(7)]
    // Spawn point to instantiate the enemies at here
    public Transform[] spawnPoints;

    // Enemies prefabs
    public GameObject[] enemies;

    [Header("Spawn System")]
    // Start delay for each wave spawns
    public int startDelay = 3;

    // Spwan the next enemy delay
    public float spawnDelay = 1f;

    [Header("Waves System")]
    // Wave spawning system
    public int totalWaves = 5;

    // Display total waves
    public Text wavesText;

    // The next wave start counter text
    public Text nextWaveText;

    // Each waves spawn's limits
    public int[] wavesSpawnLimits;

    // Internal variables
    bool canSpawn = true;
    bool gameStarted;
    Transform currentPoint;
    int index;
    [HideInInspector] public GameObject[] temp;
    GameManager gameManager;
    // Waves system
    [HideInInspector] public GameObject[] allObjects;
    [HideInInspector] public List<GameObject> spawnedEnemies;
    int currentWave = 0;
    int spawnedCounts = 0;



    IEnumerator Start()
    {
        // get GameManager component to display the you win window
        gameManager = GameObject.FindObjectOfType<GameManager>();

        // Update the wave's data
        currentWave = currentWave + 1;
        if (wavesText)
            wavesText.text = "Waves : " + currentWave.ToString() + " / " + totalWaves.ToString();

        // Winner
        if (currentWave > totalWaves)
        {
            wavesText.text = "Waves : " + totalWaves.ToString() + " / " + totalWaves.ToString();
            GameObject.FindObjectOfType<GameManager>().You_Win();
        }

        StartCoroutine(Next_Wave_Counter());

        // Spawn start delay for each wave
        yield return new WaitForSeconds(startDelay);

        // The game / wave is now started, you can start spawing
        gameStarted = true;
        canSpawn = true;

        // Temps the spawns
        spawnedCounts = 0;

        // Check that the current wave ends
        StartCoroutine(CheckSpawns());

        // Spawn the enemies based on the spawn delay time and spawn limit
        while (canSpawn)
        {
            // List all spawned enemies to limit the spawns based on the spawn limit value
            temp = GameObject.FindGameObjectsWithTag("Enemy");

            // Limited spawner
            if (spawnedCounts < wavesSpawnLimits[currentWave - 1])
            {
                // Random spawns points selection
                index = Random.Range(0, spawnPoints.Length);
                currentPoint = spawnPoints[index];
                GameObject enemySpawned = Instantiate(enemies[index], currentPoint.position, Quaternion.identity);
                enemySpawned.name = "Enemy_Wave_" + currentWave.ToString();
                spawnedCounts++;
            }
            else
            {
                // Spawns limited
                canSpawn = false;
            }

            // Delay before spawning the next enemy in the current wave
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Check spawns to determine the next wave (go to next wave when all enemies destroyed)
    IEnumerator CheckSpawns()
    {
        while (true)
        {
            // Use delay to get better performance
            yield return new WaitForSeconds(1f);

            if (gameStarted)
            {
                CountEnemy();
            }
        }
    }

    // Count spawned enemies to know when go to the next wave
    void CountEnemy()
    {
        allObjects = GameObject.FindGameObjectsWithTag("Enemy");

        spawnedEnemies.Clear();

        // Find the current waves enemy and add to the "spawnedEnemies" game object list
        for (int a = 0; a < allObjects.Length; a++)
        {
            if (allObjects[a].name == ("Enemy_Wave_" + currentWave.ToString()))
            {
                spawnedEnemies.Add(allObjects[a]);
            }
        }
        // Go to the next wave
        if (spawnedEnemies.Count == 0)
        {
            gameStarted = false;
            StartCoroutine(Start());
        }
    }

    // All waves has been passed... the palyer is now a wiinner
    IEnumerator Next_Wave_Counter()
    {
        // Show the next wave counter
        nextWaveText.gameObject.SetActive(true);
        int counts = startDelay;

        PlayerPrefs.SetInt("Total Waves Passed", PlayerPrefs.GetInt("Total Waves Passed") + 1);

        while (!gameStarted)
        {
            nextWaveText.text = "Next Wave Start : " + counts.ToString();
            counts--;

            if (currentWave > totalWaves)
                nextWaveText.text = "Waves Completed...";

            yield return new WaitForSeconds(1f);

        }
        // The next wave is now started
        nextWaveText.gameObject.SetActive(false);

    }
}
