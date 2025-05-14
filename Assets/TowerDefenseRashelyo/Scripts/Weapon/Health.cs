// AliyerEdon@mail.com Christmas 2022
// Add this component to your every actor that did you like to have health and dead features

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Type of the actor
public enum TargetType
{
    Enemy, Tower, Defender
}

public class Health : MonoBehaviour
{
    [Space(5)]
    [Header("General Settings")]
    [Space(7)]
    // Type of the target actor
    public TargetType targetType;


    // How much award coins give to the player after destroying this actor
    public int destroyAwardedCoins = 100;

    // Instantiate this game object after destroying the actor
    public GameObject damageParticle;

    // Wait before destroying the actor's game object
    public float destroyDelay = 0;
    GameManager gManager;



    // Internal variables
    bool isDead;

    [Space(5)]
    [Header("Health Bar")]
    // Total health value
    public int healthValue = 100;
    public int maxHealthValue;
    // Health bar for enemy actors
    public Slider healthColor;

    void Start()
    {
        // Get access to the game manager to update the coins display
        gManager = GameObject.FindObjectOfType<GameManager>();

        // Initialize the health slider with max health
        if (healthColor)
            healthColor.maxValue = maxHealthValue;

        // Set initial health value on slider
        if (healthColor)
            healthColor.value = healthValue;
    }

    // Apply damage to the actor
    public void ApplyDamage(int damage)
    {
        // Reduce the health value
        healthValue = Mathf.Clamp(healthValue - damage, 0, maxHealthValue);

        // Update the health bar game object
        #region Health bar
        if (healthColor)
        {
            // Update the health slider UI
            healthColor.value = healthValue; // Set the slider value according to the current health
        }
        #endregion

        // Dead
        #region Enemy
        if (targetType == TargetType.Enemy)
        {
            if (healthValue <= 0)
            {
                if (GetComponent<CapsuleCollider>())
                    GetComponent<CapsuleCollider>().enabled = false;

                GetComponent<Weapon>().canShoot = false;

                // Add coins when the enemy was destroyed
                gManager.AddCoins(destroyAwardedCoins);

                // Instantiate the destroys particle
                if (damageParticle)
                    Instantiate(damageParticle, transform.position, transform.rotation);

                // Destroy the health bar game object
                Destroy(healthColor.transform.parent.gameObject);

                // Stop shooting the weapon
                GetComponent<Weapon>().shootingDelay = 1000f;

                // Play animations if was available
                if (GetComponent<AnimationList>().actor)
                {
                    GetComponent<Weapon>().canShoot = false;

                    if (!isDead)
                    {
                        if (GetComponent<AnimationList>().actor)
                            StartCoroutine(PlayDeadAnimation());
                    }

                    isDead = true;

                }
                else
                {
                    // Destroy by delay (let the animation to be ended)
                    StartCoroutine(Destroy_Delay());
                }
            }
        }
        #endregion

        #region Tower
        if (targetType == TargetType.Tower)
        {
            // Reduce tower's health after hitting by enemy's weapon
            gManager.Reduce_Tower_Health(gManager.towerDamage);

            // Tower is destroyed
            if (healthValue <= 0)
            {
                // Instantiate the destroyed particle if it was assigned in the inspector
                if (damageParticle)
                    Instantiate(damageParticle, transform.position, transform.rotation);

            }
        }
        #endregion

        #region Defender
        if (targetType == TargetType.Defender)
        {
            // Tower is destroyed
            if (healthValue <= 0)
            {
                if (damageParticle)
                    Instantiate(damageParticle, transform.position, transform.rotation);

                // Play its animation if it was available
                if (GetComponent<AnimationList>().actor)
                {
                    if (!isDead)
                    {
                        if (GetComponent<AnimationList>().actor)
                            StartCoroutine(PlayDeadAnimation());
                    }
                    isDead = true;
                }
                else
                {
                    // Destroy by delay (let the animation to be ended)
                    StartCoroutine(Destroy_Delay());
                }
            }
        }
        #endregion
    }

    // Manage the dead animation
    IEnumerator PlayDeadAnimation()
    {
        // If the dead animation was available, just play it
        if (GetComponent<AnimationList>().actor)
        {
            GetComponent<AnimationList>().actor.CrossFade(GetComponent<AnimationList>().deadClip);
        }

        // Stop the nav mesh agent movement
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        transform.tag = "Untagged";

        // Wait 5 seconds to dead animation plays
        yield return new WaitForSeconds(5);

        if (targetType == TargetType.Enemy)
            PlayerPrefs.SetInt("Total Kills", PlayerPrefs.GetInt("Total Kills") + 1);

        // Destroy the actor completely
        Destroy(gameObject);
    }

    // Destroy actor after specific delay
    IEnumerator Destroy_Delay()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Add totals kills for trophies system
        if (targetType == TargetType.Enemy)
            PlayerPrefs.SetInt("Total Kills", PlayerPrefs.GetInt("Total Kills") + 1);

        Destroy(gameObject);
    }
}
