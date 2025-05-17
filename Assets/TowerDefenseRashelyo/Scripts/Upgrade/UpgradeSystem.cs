// AliyerEdon@mail.com Christmas 2022
// Use this to manage upgrade system for defender's power upgrades and the tower health the the main menu

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{


	// Temp for upgrades
	int[] defenderUpgradeLevel;
	int towerUpgradeLevel;

	[Header("Defender Upgrades =>")]
	[Space(7)]
	// Max upgrade level for all defenders
	public int defenderMaxUpgradeLevel = 3;

	[Space(7)]
	// Use this to display the upgrade's level (example: 1 / 3)
	public Text[] defenderUpgradesInfo;

	// Each defender's upgrade price
	public int[] defenderUpgradesPrice;

	// Use this to display each defender's upgrade price
	public Text[] defenderUpgradesPriceInfo;

	[Header("Tower Upgrades =>")]
	[Space(7)]
	// Max upgrade level for the tower (tower health)
	public int towerMaxUpgradeLevel = 3;

	[Space(7)]
	// Use this to display the upgrade's level (example: 1 / 3)
	public Text towerUpgradesInfo;

	// Each tower's upgrade price
	public int towerUpgradesPrice;

	// Use this to display each tower's upgrade price
	public Text towerUpgradesPriceInfo;

	[Header("User Interface =>")]
	[Space(7)]
	// Display shop menu if the player has not enough coins (watch the video to earn coins)
	public GameObject shopMenu;

	void Start()
	{

		// Load upgrades on start 
		LoadUpgrade();
	}

	// Load upgrades
	public void LoadUpgrade()
	{

		defenderUpgradeLevel = new int[defenderUpgradesInfo.Length];

		// Load the defender's upgrades level (save to tDefender array) example: 1 / 10
		for (int a = 0; a < defenderUpgradesInfo.Length; a++)
		{
			defenderUpgradeLevel[a] = PlayerPrefs.GetInt("Defender" + a.ToString());
			defenderUpgradesInfo[a].text = "Level : " + defenderUpgradeLevel[a].ToString() + " / " + defenderMaxUpgradeLevel.ToString();
		}

		// Load the tower upgrades (save to tTower) example : 1 / 10
		// towerUpgradeLevel = PlayerPrefs.GetInt("Tower");
		// towerUpgradesInfo.text = "Level : " + towerUpgradeLevel.ToString() + " / " + towerMaxUpgradeLevel.ToString();

		// update total coins display text (UI.Text)
		Update_Coins_Display();

		// Load the defender upgrades prices (save to defenderUpgradesPriceInfo array)
		for (int a = 0; a < defenderUpgradesPriceInfo.Length; a++)
		{
			if (defenderUpgradeLevel[a] < defenderMaxUpgradeLevel)
				defenderUpgradesPriceInfo[a].text = defenderUpgradesPrice[a].ToString() + " $";
			else
				defenderUpgradesPriceInfo[a].text = "Completed";
		}

		// Load the tower upgrades prices (save to towerUpgradesPriceInfo array)
		// if (towerUpgradeLevel < towerMaxUpgradeLevel)
		// 	towerUpgradesPriceInfo.text = towerUpgradesPrice.ToString() + " $";
		// else
		// 	towerUpgradesPriceInfo.text = "Completed";

	}

	// Upgrade Defenders
	public void Defender_Upgrade(int id)
	{
		AudioEventSystem.PlayAudio("OpenMenu");
		if (defenderUpgradeLevel[id] < defenderMaxUpgradeLevel)
		{
			if (PlayerPrefs.GetInt("Total Coins") >= defenderUpgradesPrice[id])
			{
				PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Total Coins") - defenderUpgradesPrice[id]);
				defenderUpgradeLevel[id]++;
				PlayerPrefs.SetInt("Defender" + id.ToString(), defenderUpgradeLevel[id]);
				Update_Coins_Display();
				defenderUpgradesInfo[id].text = "Level : " + defenderUpgradeLevel[id].ToString() + " / " + defenderMaxUpgradeLevel.ToString();

				if (defenderUpgradeLevel[id] < defenderMaxUpgradeLevel)
					defenderUpgradesPriceInfo[id].text = defenderUpgradesPrice[id].ToString() + " $";
				else
					defenderUpgradesPriceInfo[id].text = "Completed";

			}
			else
			{
				if (shopMenu)
					shopMenu.SetActive(true);
			}

		}
	}

	public void Tower_Upgrade()
	{
		if (towerUpgradeLevel < towerMaxUpgradeLevel)
		{
			if (PlayerPrefs.GetInt("Total Coins") >= towerUpgradesPrice)
			{
				PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Total Coins") - towerUpgradesPrice);
				towerUpgradeLevel++;
				PlayerPrefs.SetInt("Tower", towerUpgradeLevel);
				Update_Coins_Display();
				towerUpgradesInfo.text = "Level : " + towerUpgradeLevel.ToString() + " / " + towerMaxUpgradeLevel.ToString();

				if (towerUpgradeLevel < towerMaxUpgradeLevel)
					towerUpgradesPriceInfo.text = towerUpgradesPrice.ToString() + " $";
				else
					towerUpgradesPriceInfo.text = "Completed";

			}
			else
			{
				if (shopMenu)
					shopMenu.SetActive(true);
			}

		}
	}

	public void Update_Coins_Display()
	{
		GameObject.FindObjectOfType<MainMenu>().totalCoinsText.text =
			PlayerPrefs.GetInt("Total Coins").ToString();
	}
}
