// AliyerEdon@mail.com Christmas 2022
// Trophies system (load and display in the menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trophies : MonoBehaviour
{
    // Trophies list
    public Image kills_1000, kills_5000, kills_10000, kills_100000;
    public Image scores_100000, scores_1000000;
    public Image wave_100, wave_1000;
    public Image unlock_Level4;


    // Start is called before the first frame update
    void Start()
    {
        Update_Trophies();
    }

    // Update is called once per frame
    public void Update_Trophies()
    {
        if (PlayerPrefs.GetInt("Total Kills") >= 1000) // 1 = true , 0 = false
            kills_1000.color = Color.green;
        else
            kills_1000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Kills") >= 5000) // 1 = true , 0 = false
            kills_5000.color = Color.green;
        else
            kills_5000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Kills") >= 10000) // 1 = true , 0 = false
            kills_10000.color = Color.green;
        else
            kills_10000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Kills") >= 100000) // 1 = true , 0 = false
            kills_100000.color = Color.green;
        else
            kills_100000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Scores") >= 100000) // 1 = true , 0 = false
            scores_100000.color = Color.green;
        else
            scores_100000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Scores") >= 1000000) // 1 = true , 0 = false
            scores_1000000.color = Color.green;
        else
            scores_1000000.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Level Unlocked3") == 1) // 1 = true , 0 = false
            unlock_Level4.color = Color.green;
        else
            unlock_Level4.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Waves Passed") >= 100) // 1 = true , 0 = false
            wave_100.color = Color.green;
        else
            wave_100.color = Color.red;
        //------------------------------------------------
        if (PlayerPrefs.GetInt("Total Waves Passed") >= 1000) // 1 = true , 0 = false
            wave_1000.color = Color.green;
        else
            wave_1000.color = Color.red;
        //------------------------------------------------
    }
}
