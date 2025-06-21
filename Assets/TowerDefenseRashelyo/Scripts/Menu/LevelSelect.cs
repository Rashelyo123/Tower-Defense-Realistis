using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Space(7)]
    [Header("Level Selection")]
    public string[] LevelsName;
    public int[] LevelsPrice;
    public TextMeshProUGUI[] LevelsPriceText;
    public GameObject[] LevelsLock;

    [Space(7)]
    [Header("Display Menu")]
    public GameObject LoadingWindow;
    public GameObject buylevelWindow;
    public Slider progressBar; // Slider untuk progress bar
    public Text ProgressText; // Teks untuk menampilkan persentase

    int purchaseLevelID;

    private void Start()
    {
        UpdateLevelUI();
    }

    private void OnEnable()
    {
        UpdateLevelUI();
    }

    private void UpdateLevelUI()
    {
        for (int a = 0; a < LevelsLock.Length; a++)
        {
            if (LevelsPrice[a] == 0)
            {
                PlayerPrefs.SetInt("Level Unlocked " + a.ToString(), 1);
            }

            if (PlayerPrefs.GetInt("Level Unlocked " + a.ToString()) == 1)
            {
                LevelsLock[a].SetActive(false);
            }
            else
            {
                LevelsLock[a].SetActive(true);
            }

            LevelsPriceText[a].text = LevelsPrice[a].ToString() + " Coins";
        }
    }

    public void Select_Level(int id)
    {
        if (PlayerPrefs.GetInt("Level Unlocked " + id.ToString()) == 1)
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

    public void Buy_Level()
    {
        if (PlayerPrefs.GetInt("Total Coins") >= LevelsPrice[purchaseLevelID])
        {
            PlayerPrefs.SetInt("Total Coins", PlayerPrefs.GetInt("Total Coins") - LevelsPrice[purchaseLevelID]);
            PlayerPrefs.SetInt("Level Unlocked " + purchaseLevelID.ToString(), 1);
            buylevelWindow.SetActive(false);
            UpdateLevelUI();
            GameObject.FindObjectOfType<MainMenu>().totalCoinsText.text = PlayerPrefs.GetInt("Total Coins").ToString();
        }
    }

    private IEnumerator PrepereLevel(int id)
    {
        LoadingWindow.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelsName[id]);
        asyncLoad.allowSceneActivation = false;

        float displayedProgress = 0f; // Nilai progress yang ditampilkan di UI
        float lerpSpeed = 2f; // Kecepatan interpolasi (semakin kecil, semakin lambat)

        while (!asyncLoad.isDone)
        {
            float actualProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            // Interpolasi nilai progress untuk efek lambat
            displayedProgress = Mathf.Lerp(displayedProgress, actualProgress, Time.deltaTime * lerpSpeed);

            // Perbarui UI
            progressBar.value = displayedProgress;
            ProgressText.text = "Loading: " + (displayedProgress * 100).ToString("F0") + "%";

            // Debug untuk memeriksa progress
            Debug.Log("Actual Progress: " + (actualProgress * 100) + "%, Displayed Progress: " + (displayedProgress * 100) + "%");

            if (asyncLoad.progress >= 0.9f && displayedProgress >= 0.99f)
            {
                yield return new WaitForSeconds(1f); // Penundaan tambahan untuk animasi
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}