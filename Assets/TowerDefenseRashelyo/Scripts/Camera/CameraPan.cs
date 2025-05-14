// AliyerEdon@mail.com Christmas 2022
// Attach this component to your camera to have the "Pan" feature in the limited  area (between min and max

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Space(5)]
    [Header("Camera Pan System")]
    public float mouseSensitivity = 1.0f;
    Vector3 lastPosition ;

    public float limitLeft,limitRight;
    GameManager gameManager;

    private void Start()
    {
        // Find and load game manager to get the mouse is drags
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void Update()
    {
        // Save the last position of the mouse
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            lastPosition = Input.mousePosition;
        }

        // The mouse left button down (single finger touch)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Drag only the mouse (single finger touch) is only dragged on the non-gui section of the screen
            if (gameManager.dragOnViewSpace)
            {
                Vector3 delta = Input.mousePosition - lastPosition;
                Vector3 CameralastLocation = gameObject.transform.position;

                // Pan the camera using translate function of its transform
                transform.Translate(delta.x * mouseSensitivity, 0, 0);

                // Limit the pan area (left and right)
                if (transform.position.x > limitRight)
                    transform.position = new Vector3(CameralastLocation.x, transform.position.y, transform.position.z);
                if (transform.position.x < limitLeft)
                    transform.position = new Vector3(CameralastLocation.x, transform.position.y, transform.position.z);

                // Update the last position
                lastPosition = Input.mousePosition;
            }
        }
    }
}
