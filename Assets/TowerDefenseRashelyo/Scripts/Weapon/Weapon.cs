// AliyerEdon@mail.com Christmas 2022
// use this component to set up your own customized weapon for every actor (enemy, defender)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Weapon type (use gun for shooter games, use sword for other types)
public enum ShootingMode
{
    Gun, Sword
}


public class Weapon : MonoBehaviour
{
    [Space(5)]
    [Header("General Settings")]
    public ShootingMode shootingMode = ShootingMode.Gun;

    // Projectile (or bullet) to spawn
    public GameObject projectile;
    public Animator anim;

    // particle system
    public ParticleSystem shootingEffect;

    // Instantiate the projectile and add force from this point
    public Transform shootPoint;

    // Add force to the projectile(or bullet)
    public float force = 100f;

    // Firing rate
    public float shootingDelay = 1f;

    [Space(5)]
    [Header("Sword Settings")]
    public int SwordDamage = 1;

    [Space(5)]
    [Header("Additional Options")]
    // If your actor has 2 guns, use this options
    public GameObject secondProjectile;
    public Transform secondShootPoint;

    [Space(5)]
    [Header("Sound Settings")]
    public AudioClip fireClip;
    AudioSource audioSource;

    [HideInInspector] public bool canShoot = false;

    GameManager gameManager;


    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        shootingEffect = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        if (shootingEffect == null)
        {
            shootingEffect = GetComponentInChildren<ParticleSystem>();
        }
        shootingEffect.Stop();

        // Use game manager to apply damage
        gameManager = GameObject.FindObjectOfType<GameManager>();

        while (true)
        {
            // Delay before each fire
            yield return new WaitForSeconds(shootingDelay);

            if (canShoot)
            {
                // Gun , weapon fire
                if (shootingMode == ShootingMode.Gun)
                {
                    // Instantiate bullet
                    GameObject bullet = Instantiate(projectile, shootPoint.position, shootPoint.rotation) as GameObject;

                    // Add force to the Instantiated bullet
                    bullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * force);

                    // Double projectile mode (useful for double weapon's turret)
                    if (secondProjectile)
                    {
                        GameObject bullet2 = Instantiate(secondProjectile, secondShootPoint.position, secondShootPoint.rotation) as GameObject;
                        bullet2.GetComponent<Rigidbody>().AddForce(secondShootPoint.forward * force);
                    }

                    // Play fire sound
                    if (shootingMode == ShootingMode.Gun)
                    {
                        if (audioSource && fireClip)
                            audioSource.PlayOneShot(fireClip);
                        if (anim != null)
                        {
                            anim.SetTrigger("Fire");
                        }
                        //play particle effect when fire
                        if (shootingEffect != null && anim != null)
                        {
                            shootingEffect.Play();
                            StartCoroutine(StopShootingEffect()); // Stop the effect after a delay

                        }
                        else if (shootingEffect != null)
                        {
                            shootingEffect.Play();
                            StartCoroutine(StopShootingEffect()); // Stop the effect after a delay


                        }

                    }

                    if (GetComponent<AnimationList>())
                    {
                        if (GetComponent<AnimationList>().actor)
                        {
                            GetComponent<AnimationList>().actor.CrossFade(GetComponent<AnimationList>().fireClip);
                        }
                    }
                }
                // Apply damage by sword hit
                if (shootingMode == ShootingMode.Sword)
                {
                    if (GetComponent<AnimationList>().actor)
                    {
                        GetComponent<AnimationList>().actor.CrossFade(GetComponent<AnimationList>().fireClip);
                    }
                    // Reduce the target's health
                    gameManager.Reduce_Tower_Health(SwordDamage);
                }
            }
        }
    }
    // Coroutine to stop particle effect after delay
    IEnumerator StopShootingEffect()
    {
        yield return new WaitForSeconds(shootingDelay);  // Wait for the delay before stopping
        if (shootingEffect != null && shootingEffect.isPlaying)
        {
            shootingEffect.Stop();

        }
    }
}
