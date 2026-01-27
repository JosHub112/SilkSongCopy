using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    // Shake parameters
    public float shakeDuration = 0.08f; // How long the shake lasts
    public float shakeMagnitude = 0.04f; // How much it shakes

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Start shake
        StopAllCoroutines(); // Stop previous shakes if overlapping
        StartCoroutine(Shake());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Random offset added to current position
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            randomOffset.z = 0; // Keep shake in 2D plane
            transform.position += randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}