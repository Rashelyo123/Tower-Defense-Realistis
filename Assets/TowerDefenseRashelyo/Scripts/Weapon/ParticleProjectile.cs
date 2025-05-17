using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjectile : MonoBehaviour
{
    public string targetTag = "Enemy";
    public int damageValue;
    public Vector3 MaximumAngularCameraShake;
    public AudioClip hitSound;

    void OnParticleCollision(GameObject other)
    {
        Camera.main.GetComponent<StressReceiver>().InduceStress(1.0f);
        Camera.main.GetComponent<StressReceiver>().MaximumAngularShake = MaximumAngularCameraShake;
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
