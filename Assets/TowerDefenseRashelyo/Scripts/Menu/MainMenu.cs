using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Space(7)]
    [Header("Play the game and press -H- key to delete game's save data")]
    public int startingCoins = 1000;
    public int minimumCoins = 1000;
    public Text totalCoinsText;
    public Text totalScoresText;
    //UI Menu
    public GameObject UpgradeMenu;
    public GameObject SettingsMenu;
    public GameObject SatisticsMenu;
    public GameObject playMenu;
    public GameObject TropyMenu;
    public GameObject ScoreMenu;
    public GameObject CoinMenu;
    public GameObject JudulGame;
    // Sound
    public AudioSource buttonClickSound;


    void Start()
    {
        if (PlayerPrefs.GetInt("FirstRun") != 1)
        {
            PlayerPrefs.SetInt("Total Coins", startingCoins);
            PlayerPrefs.SetInt("Minimum Coins", minimumCoins);
            PlayerPrefs.SetInt("Level Unlocked0", 1);
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.SetInt("Music", 1);
        }

        if (PlayerPrefs.GetInt("Total Coins") < PlayerPrefs.GetInt("Minimum Coins"))
            PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Minimum Coins"));

        totalCoinsText.text = PlayerPrefs.GetInt("Total Coins").ToString();
        totalScoresText.text = PlayerPrefs.GetInt("Total Scores").ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Game's saved data deleted successfully");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Enable_Object(GameObject target)
    {
        target.SetActive(true);
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }

    public void Disable_Object(GameObject target)
    {
        target.SetActive(false);
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }

    private void EnableAllMenus()
    {
        UpgradeMenu.SetActive(true);
        SettingsMenu.SetActive(true);
        SatisticsMenu.SetActive(true);
        playMenu.SetActive(true);
        TropyMenu.SetActive(true);
        ScoreMenu.SetActive(true);
        CoinMenu.SetActive(true);
        JudulGame.SetActive(true);
    }

    private void DisableAllMenus()
    {
        UpgradeMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SatisticsMenu.SetActive(false);
        playMenu.SetActive(false);
        TropyMenu.SetActive(false);
        ScoreMenu.SetActive(false);
        CoinMenu.SetActive(false);
        JudulGame.SetActive(false);
    }

    public void Toggle_Object(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }

    public void EnableOnlyThisMenu(GameObject target)
    {
        DisableAllMenus();
        target.SetActive(true);

    }

    public void DisableOnlyThisMenu(GameObject target)
    {
        EnableAllMenus();
        target.SetActive(false);

    }
}
