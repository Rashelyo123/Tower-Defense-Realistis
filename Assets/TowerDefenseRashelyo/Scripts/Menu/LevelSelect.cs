// AliyerEdon@mail.com Christmas 2022
// Manage the level's lock system
// *** Use this in the main menu scene t manage the levels locks / unlocks

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Space(7)]
    [Header("Level Selection")]
    public string[] LevelsName;
    public int[] LevelsPrice;
    public Text[] LevelsPriceText;
    public GameObject[] LevelsLock;


    [Space(7)]
    [Header("Display Menu")]
    public GameObject LoadingWindow;
    public GameObject buylevelWindow;

    int currentLevelForPurchase;

    private void Start()
    {
        for (int a = 0; a < LevelsLock.Length; a++)
        {
            if (PlayerPrefs.GetInt("Level Unlocked " + a.ToString()) == 1)

                LevelsLock[a].SetActive(false);
            else
                LevelsLock[a].SetActive(true);
            LevelsPriceText[a].text = LevelsPrice[a].ToString() + " Coins";


        }
    }

    public void Select_Level(int id)
    {
        if (PlayerPrefs.GetInt("Level Unlocked" + id.ToString()) == 1)
        {
            PlayerPrefs.SetInt("Current Level", id);
            StartCoroutine(PrepereLevel(id));


        }
        else
        {
            purchaseLevelID = id;
            buylevelWindow.SetActive(true);

        }
    }
    int purchaseLevelID;

    public void Buy_Level()
    {
        if (PlayerPrefs.GetInt("Total Coins") >= LevelsPrice[purchaseLevelID])
        {
            PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Total Coins") - LevelsPrice[purchaseLevelID]);
            LevelsLock[purchaseLevelID].SetActive(false);
            GameObject.FindObjectOfType<MainMenu>().totalCoinsText.text = PlayerPrefs.GetInt("Total Coins").ToString();
            PlayerPrefs.SetInt("Level Unlocked" + purchaseLevelID.ToString(), 1);
            buylevelWindow.SetActive(false);

        }
    }

    private IEnumerator PrepereLevel(int id)
    {
        LoadingWindow.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(LevelsName[id]);
    }
}
