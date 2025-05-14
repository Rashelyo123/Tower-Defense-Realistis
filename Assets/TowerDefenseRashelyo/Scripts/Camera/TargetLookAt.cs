// AliyerEdon@mail.com Christmas 2022
// Use this to look any game object to the camera
// *** We used this component to face the health bar to the camera

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLookAt : MonoBehaviour
{

    // use update interval to optimize the update function (higher interval has better perofmance)
    public float updateInterval = 0.1f;

    IEnumerator Start()
    {
        // Update mode with the interval delay
        while(true)
        {
            transform.LookAt(Camera.main.transform, Vector3.up);
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
