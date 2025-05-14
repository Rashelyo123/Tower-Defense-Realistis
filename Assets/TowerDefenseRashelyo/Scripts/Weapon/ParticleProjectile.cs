using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjectile : MonoBehaviour
{
    public string targetTag = "Enemy";
    public int damageValue;
    public AudioClip hitSound;

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(targetTag))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ApplyDamage(damageValue);
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

        }

    }
}
