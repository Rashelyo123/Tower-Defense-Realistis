// AliyerEdon@mail.com Christmas 2022
// Use this to manage the pause menu system (load a scene, enable or disable game objects...)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Load level by pressing the exit or retry button
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start the pause action (by pressing the pause button)
    public void StartPause()
    {
        Time.timeScale = 0;
    }

    // Stop the pause action (by pressing continue, retry or exit buttons)
    public void EndPause()
    {
        Time.timeScale = 1f;
    }

    /// // Use this to enable any game object by clicking on a UI button
    public void Enable_Object(GameObject target)
    {
        target.SetActive(true);
    }

    /// // Use this to disable any game object by clicking on a UI button
    public void Disable_Object(GameObject target)
    {
        target.SetActive(false);
    }
    
    /// // Use this to toggle enable / disable any game object by clicking on a UI button
    public void Toggle_Object(GameObject target)   
    {
        target.SetActive(!target.activeSelf);
    }

    bool doubleSpeed = false;
    public void Double_Speed_Mode()
    {
        doubleSpeed = !doubleSpeed;
        if (doubleSpeed)
            Time.timeScale = 2f;
        else
            Time.timeScale = 1f;
    }
}
