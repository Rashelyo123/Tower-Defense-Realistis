// AliyerEdon@mail.com Christmas 2022
// Load item's upgrads that has been purchased / applied in the main menu

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load_Tower_Upgrades : MonoBehaviour {

	public Slider towerHealth;

	void Start () {
	
		// Don't load the upgrades when the player is in the main menu scene
		if (SceneManager.GetActiveScene ().name.Contains ("Menu"))
			return;

		// Increase the tower health value
		if(PlayerPrefs.GetInt("Tower") > 0)
		{
			GetComponent<Health>().healthValue = (PlayerPrefs.GetInt("Tower")+1) * 100;
			GameObject.FindObjectOfType<GameManager>().towerHealthSlider.maxValue = GetComponent<Health>().healthValue;
			GameObject.FindObjectOfType<GameManager>().towerHealthSlider.value = GetComponent<Health>().healthValue;
			GameObject.FindObjectOfType<GameManager>().towerHealthText.text = GetComponent<Health>().healthValue.ToString();
		}
	}

}
