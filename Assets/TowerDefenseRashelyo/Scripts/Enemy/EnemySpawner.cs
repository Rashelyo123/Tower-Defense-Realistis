using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Space(7)]
    [Header("Spawn Points")]
    // Spawn point to instantiate the enemies at here, paired with WaypointSystem
    public Transform[] spawnPoints;
    public WaypointSystem[] waypointSystems; // Array untuk menyimpan WaypointSystem terkait

    // Enemies prefabs
    public GameObject[] enemies;

    [Header("Spawn System")]
    // Start delay for each wave spawns
    public int startDelay = 3;

    // Spawn the next enemy delay
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
    [HideInInspector] public List<GameObject> spawnedEnemies = new List<GameObject>();
    int currentWave = 0;
    int spawnedCounts = 0;

    void Start()
    {
        // Validasi bahwa jumlah spawnPoints dan waypointSystems sama
        if (spawnPoints.Length != waypointSystems.Length)
        {
            Debug.LogError($"Jumlah spawnPoints ({spawnPoints.Length}) tidak sama dengan jumlah waypointSystems ({waypointSystems.Length}) pada {gameObject.name}!");
        }
        StartCoroutine(StartCoroutine());
    }

    IEnumerator StartCoroutine()
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

        // The game / wave is now started, you can start spawning
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
                GameObject enemySpawned = Instantiate(enemies[Random.Range(0, enemies.Length)], currentPoint.position, Quaternion.identity);
                enemySpawned.name = "Enemy_Wave_" + currentWave.ToString();
                spawnedEnemies.Add(enemySpawned);

                // Atur WaypointSystem secara acak untuk musuh
                NavMover navMover = enemySpawned.GetComponent<NavMover>();
                if (navMover != null && index < waypointSystems.Length)
                {
                    navMover.SetPath(waypointSystems[index]);
                    navMover.spawner = this; // Atur referensi spawner
                }

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
            StartCoroutine(StartCoroutine());
        }
    }

    // All waves has been passed... the player is now a winner
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

            if (currentWave == totalWaves)
            {
                nextWaveText.text = "Disaster Is Coming : " + counts.ToString();
            }

            if (currentWave > totalWaves)
                nextWaveText.text = "Waves Completed...";

            yield return new WaitForSeconds(1f);
        }
        // The next wave is now started
        nextWaveText.gameObject.SetActive(false);
    }

    // Dipanggil saat musuh mencapai akhir jalur
    public void OnEnemyReachedEnd(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}