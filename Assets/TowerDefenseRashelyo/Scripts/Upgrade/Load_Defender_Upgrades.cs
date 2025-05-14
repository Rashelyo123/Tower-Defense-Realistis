// AliyerEdon@mail.com Christmas 2022
// Load item's upgrads that has been purchased / applied in the main menu

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Load_Defender_Upgrades : MonoBehaviour {

	public int defenderId = 0;

	void Start () {
	
		// Don't load the upgrades when the player is in the main menu scene
		if (SceneManager.GetActiveScene ().name.Contains ("Menu"))
			return;

		// Reduce shoting dalay based on the defender's upgrade
			GetComponent<Weapon>().shootingDelay = GetComponent<Weapon>().shootingDelay
				 / (PlayerPrefs.GetInt("Defender" + defenderId.ToString())+1);
		}

}
