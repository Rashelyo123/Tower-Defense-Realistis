// AliyerEdon@mail.com Christmas 2022
// This is the settings system in the main menu of the game
// *** Use settings loader script in your scene to load the selected / saved settings

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //___________________________________________
    // Toggles to select the game settings
    public Toggle Effect, Music;

    // Dropdown for quality selection
    public Dropdown qualityLevel;
    //___________________________________________

    void Start()
    {
        // Load the post effect's state (save before)
        if (PlayerPrefs.GetInt("Effect") == 1) // 1 = true , 0 = false        
            Effect.isOn = true;
        else
            Effect.isOn = false;
        //___________________________________________

        // Load the music's state (save before)
        if (PlayerPrefs.GetInt("Music") == 1) // 1 = true , 0 = false        
            Music.isOn = true;
        else
            Music.isOn = false;
        //___________________________________________
        // Load the quality level (saved before)
        qualityLevel.value = PlayerPrefs.GetInt("QualityLevel");
        //___________________________________________
    }

    #region Effect
    //___________________________________________
    // Use this on the UI.Toggle on value changs event
    public void Toggle_Effect()
    {
        StartCoroutine(Save_Effect());
    }
    //___________________________________________
    IEnumerator Save_Effect()
    {
        yield return new WaitForSeconds(0.01f);
        if(Effect.isOn)
            PlayerPrefs.SetInt("Effect", 1);
        else
            PlayerPrefs.SetInt("Effect", 0);
    }
    //___________________________________________
    #endregion

    #region Music
    //___________________________________________
    // Use this on the UI.Toggle on value changs event
    public void Toggle_Music()
    {
        StartCoroutine(Save_Music());
    }
    //___________________________________________
    IEnumerator Save_Music()
    {
        yield return new WaitForSeconds(0.01f);
        if (Music.isOn)
            PlayerPrefs.SetInt("Music", 1);
        else
            PlayerPrefs.SetInt("Music", 0);
    }
    //___________________________________________
    #endregion

    #region Quality
    //___________________________________________
    // Use this on the UI.Dropdown on value changes event
    public void Toggle_Quality()
    {
        StartCoroutine(Save_Quality());
    }
    //___________________________________________
    IEnumerator Save_Quality()
    {
        yield return new WaitForSeconds(0.01f);     
        
        PlayerPrefs.SetInt("QualityLevel", qualityLevel.value);

        QualitySettings.SetQualityLevel(qualityLevel.value);
    }
    //___________________________________________
    #endregion

}
