using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Target's tag to apply damage
    public string targetTag = "Enemy";

    // Damage value to apply to the target after collision
    public int damageValue;

    // Instantiate the damage particle after collision
    public GameObject damageParticle;

    // Destroy the projectile after 5 seconds without collision
    public float lifeTime = 5f;

    // Time to destroy damage particle after it's spawned
    public float damageParticleLifetime = 2f; // Durasi partikel sebelum dihancurkan

    IEnumerator Start()
    {
        // Destroy the projectile after lifeTime value without collision
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        // Apply damage to the target's health component
        if (col.transform.tag == targetTag)
        {
            col.transform.GetComponent<Health>().ApplyDamage(damageValue);
        }

        // Instantiate the collision particle
        if (damageParticle)
        {
            GameObject particleEffect = Instantiate(damageParticle, transform.position, transform.rotation);

            // Hancurkan partikel setelah beberapa detik (sesuai damageParticleLifetime)
            Destroy(particleEffect, damageParticleLifetime);
        }

        // Destroy the projectile (or bullet)
        Destroy(gameObject);
    }
}
