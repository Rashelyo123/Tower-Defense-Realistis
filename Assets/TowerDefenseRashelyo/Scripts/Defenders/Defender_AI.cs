// AliyerEdon@mail.com Christmas 2022
// This is the main defender AI controller (Seek...fire)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender_AI : MonoBehaviour
{
    // Gun head for rotation (seek animation and look at the target mode)
    public Transform gunHead;

    // Use damping to have smoother look at the target rotation
    public float dampingSpeed = 10f;

    // Target's tag (enemy)
    public string targetTag = "Enemy";

    // Start shooting from this distance from the target enemy 
    public float shootingDistance = 30f;

    [Header("Seek Animation")]
    // Seek animation
    public bool playAnimationClip;
    public float seekSpeed = 50f;
    public float rotateAngle = 70f;

    // Internal variables
    Vector3 originalRotation;
    bool isActive;
    Transform target;

    IEnumerator Start()
    {
        // Save the original rotation of the gun head
        originalRotation = gunHead.localRotation.eulerAngles;

        while (true)
        {
            // Find the closest enemy
            target = FindClosestEnemy();

            if(target)
            {
                // Check that the distance from the enemy is in the shooting distance range
                if (Vector3.Distance(transform.position, target.position) <= shootingDistance)
                {
                    // Start attach
                    GetComponent<Weapon>().canShoot = true;
                    isActive = true;
                }
                else
                {
                    // Stop attach
                    GetComponent<Weapon>().canShoot = false;
                    isActive = false;
                }
            }
            else
            {
                // The enemy is out of the shooting range
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
        else
        {
            // Play animation clip it was available
            if(playAnimationClip)
            {
                if (GetComponent<AnimationList>().actor)
                {
                       GetComponent<AnimationList>().actor.CrossFade(GetComponent<AnimationList>().seekClip);
                }
            }
            else
            {
                // Weapon head's seek animation (when the enemy is not available or it's out of the shooting distance)        if (!isActive)
                gunHead.localRotation = Quaternion.Euler(originalRotation.x, Mathf.PingPong(Time.time * seekSpeed, rotateAngle * 2) - rotateAngle, 1f);
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
        if (gos.Length == 0)
            return null;
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
