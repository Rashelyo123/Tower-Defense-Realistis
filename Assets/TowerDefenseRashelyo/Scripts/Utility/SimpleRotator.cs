using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    // Rotation speed
    public float rotationSpeed = 300f;

    // Rotation axis
    public Vector3 axis = new Vector3(0,0,-10f);


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
