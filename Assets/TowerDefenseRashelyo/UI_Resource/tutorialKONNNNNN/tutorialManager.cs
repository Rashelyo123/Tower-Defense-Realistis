using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private GameObject tutorialPanel;




    void Start()
    {

        Time.timeScale = 0f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (tutorialPanel.activeSelf)
            {
                tutorialPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                tutorialPanel.SetActive(true);
                Time.timeScale = 0f;
            }

        }
    }
}
