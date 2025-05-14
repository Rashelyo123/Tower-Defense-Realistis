// AliyerEdon@mail.com Christmas 2022
// Load game settings that selected in the game main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{

    // Drag URP or other pipelines volume game object here
    [Space(5)]
    [Header("Post processing Volume")]
    public GameObject volume;

    // Start is called before the first frame update
    [Space(5)]
    [Header("Load Game Settings")]
    public AudioSource musicSource;

    void Start()
    {
        if (GetComponent<AudioSource>())
            musicSource = GetComponent<AudioSource>();

        #region Volume
        if(volume)
        {
            // If effect was true, activate the volume game object 
            if (PlayerPrefs.GetInt("Effect") == 1) // 1 = true , 0 = false        
                volume.SetActive(true);
            else
                volume.SetActive(false);
        }
        #endregion

        #region Music
        if (musicSource)
        {
            if (PlayerPrefs.GetInt("Music") == 1) // 1 = true , 0 = false        
                musicSource.Play();
            else
                musicSource.Stop();
        }
        #endregion
    }
}
