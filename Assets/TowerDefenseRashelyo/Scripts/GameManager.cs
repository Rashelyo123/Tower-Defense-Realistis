// AliyerEdon@mail.com Christmas 2022
// Use this to manage all gameplay design and actions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Space(7)]
    public int levelID;

    [Space(7)]
    // Defenders game objects
    public GameObject[] defenders;
    // Defender's instances
    List<GameObject> createdDefenders;
    int createdDefCounts = 0;

    // rotate defenders to look to the center of the scene


    // Coins management
    [HideInInspector] public int totalCoins;
    public int[] defendersPrice;

    [Space(7)]
    [Header("UI")]
    public Text coinsText;

    // Tower settings
    public Slider towerHealthSlider;
    public Text towerHealthText;

    // Display when Tower's health become zero (the game lost screens)
    public GameObject gameLostWindow;
    // Display when player passed thw all waves
    public GameObject gameWinWindow;

    [Space(7)]
    [Header("Tower Settings")]
    // Tower settings
    public int towerHealth = 100;
    public int towerDamage = 1;

    // internal variables
    [HideInInspector] public int currentDefender;
    [HideInInspector] public bool canInstantiate = true;
    [HideInInspector] public bool isDraging = false;
    [HideInInspector] public bool dragOnViewSpace = true;
    bool purchasedCurrentItem = true;

    void Start()
    {
        // If the total coins was less than the minimum coins, reset to the minimum coins
        if (PlayerPrefs.GetInt("Total Coins") < PlayerPrefs.GetInt("Minimum Coins"))
            PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Minimum Coins"));

        currentDefender = 1;
        purchasedCurrentItem = true;

        totalCoins = PlayerPrefs.GetInt("Total Coins");
        coinsText.text = totalCoins.ToString();

        createdDefenders = new List<GameObject>();
        canInstantiate = true;
    }
    ///////////////////////////////////////////////////
    void Update()
    {
        CreateDefender();

        EndDraging();
    }
    ///////////////////////////////////////////////////
    public void SetDefenderID(int id)
    {
        currentDefender = id;
    }
    ///////////////////////////////////////////////////
    public void SetDraging(bool dragState)
    {
        dragOnViewSpace = false;
        if (totalCoins >= defendersPrice[currentDefender - 1])
        {
            isDraging = dragState;
            canInstantiate = true;
            purchasedCurrentItem = false;
        }

    }
    ///////////////////////////////////////////////////
    public void EndDraging()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            dragOnViewSpace = true;
            isDraging = false;
            if (!purchasedCurrentItem)
            {
                totalCoins = totalCoins - defendersPrice[currentDefender - 1];
                purchasedCurrentItem = true;
            }
            PlayerPrefs.SetInt("Total Buys", PlayerPrefs.GetInt("Total Buys") + defendersPrice[currentDefender - 1]);
            PlayerPrefs.SetInt("Total Coins", totalCoins);
            coinsText.text = totalCoins.ToString();
        }
    }
    ///////////////////////////////////////////////////
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    void CreateDefender()
    {
        if (isDraging)
        {
            if (canInstantiate)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        if (!occupiedPositions.Contains(hit.point))
                        {
                            canInstantiate = false;
                            createdDefenders.Add((GameObject)Instantiate(defenders[currentDefender - 1], hit.point, Quaternion.identity));
                            createdDefCounts++;
                            occupiedPositions.Add(hit.point);
                            //Debug.Log(createdDefCounts + "..." + createdDefenders[createdDefCounts-1]);
                        }

                    }
                }
            }
            else
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        createdDefenders[createdDefCounts - 1].transform.position = hit.point;
                        Vector3 lookPos = FindClosestPoints("Center Point").position - createdDefenders[createdDefCounts - 1].transform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        createdDefenders[createdDefCounts - 1].transform.rotation = Quaternion.Slerp(createdDefenders[createdDefCounts - 1].transform.rotation, rotation, Time.deltaTime * 1000);
                    }
                }
            }
        }
    }
    ///////////////////////////////////////////////////
    public void ReduceCoins(int value)
    {
        totalCoins = totalCoins - value;
        PlayerPrefs.SetInt("Total Coins", totalCoins);
        coinsText.text = totalCoins.ToString();
    }
    ///////////////////////////////////////////////////
    public void AddCoins(int value)
    {
        totalCoins = totalCoins + value;
        PlayerPrefs.SetInt("Total Coins", totalCoins);
        PlayerPrefs.SetInt("Total Scores", PlayerPrefs.GetInt("Total Scores") + value);
        coinsText.text = totalCoins.ToString();
    }
    ///////////////////////////////////////////////////
    public void Game_Lost()
    {
        Time.timeScale = 0;
        gameLostWindow.SetActive(true);
    }

    public void You_Win()
    {
        // Use this o activate the next level by passing the current level
        PlayerPrefs.SetInt("Level Unlocked" + (levelID + 1).ToString(), 1);


        // Stop the game
        Time.timeScale = 0;
        gameWinWindow.SetActive(true);
    }
    public void Reduce_Tower_Health(int value)
    {
        towerHealth = towerHealth - value;
        towerHealthSlider.value = towerHealth;
        towerHealthText.text = towerHealth.ToString();

        if (towerHealth <= 0)
            Game_Lost();
    }

    // Find the closets scene center points
    GameObject closest;
    Transform FindClosestPoints(string tag)
    {
        GameObject[] centerPoints;
        centerPoints = GameObject.FindGameObjectsWithTag(tag);

        float distance = Mathf.Infinity;
        Vector3 position = createdDefenders[createdDefCounts - 1].transform.position;
        foreach (GameObject go in centerPoints)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest.transform;
    }
}
