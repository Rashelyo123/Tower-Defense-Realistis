using System.Collections;

using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(introooo());
    }
    public IEnumerator introooo()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Main_Menu");
    }
}

