// AliyerEdon@mail.com Christmas 2022
// Add this script to your enemy actor to add ability to attach the defenders
// *** You must add the Weapon component for your enemy actor gameobject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    // Weapon's head to look at the target
    public Transform gunHead;

    // Use damping to have smoother look at the target rotation
    public float dampingSpeed = 1f;

    // Target's tag (enemy)
    public string targetTag = "Defender";

    // Start shooting from this distance from the target enemy 
    public float shootingDistance = 30f;

    // Shoot the defebders or only shoot the tower
    public bool shootOnlyTower = true;

    // Internal variables
    Quaternion originalRotation;
    bool isActive;
    Transform target;

    IEnumerator Start()
    {

        // Save the original rotation of the gun head
        originalRotation = gunHead.rotation;

        while (true)
        {
            // Find the closest enemy (the enemy is the Tower and Defenders in this component)
            target = FindClosestEnemy();

            // Check that the distance from the enemy is in the shooting distance range
            if (Vector3.Distance(transform.position, target.position) <= shootingDistance)
            {
                // Start attach
                if (shootOnlyTower)
                {
                    if (GetComponent<NavMover>().reachedToEnd)
                    {
                        GetComponent<Weapon>().canShoot = true;
                        isActive = true;

                    }
                }
                else
                {
                    GetComponent<Weapon>().canShoot = true;
                    isActive = true;
                }
            }
            else
            {
                // Stop attach
                GetComponent<Weapon>().canShoot = false;
                isActive = false;
            }

            // Use delay to have better performance (instead of update function)
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (target)
            {
                // Look at the target code
                Vector3 lookPos = target.position - gunHead.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                gunHead.rotation = Quaternion.Slerp(gunHead.rotation, rotation, Time.deltaTime * dampingSpeed);
            }
        }
    }

    // Find the closest target with its tag (enemy)
    GameObject closest;
    Transform FindClosestEnemy()
    {
        // First find all game object to determine which one is near than others
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(targetTag);
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        // Return the closest target's transform
        return closest.transform;
    }


}
