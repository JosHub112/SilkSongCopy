using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamageTrigger : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;

    [Header("Timing")]
    public float hitCooldown = 1f;   // Time between hits per player
    public bool singleUse = false;

    // Tracks when each player can be hit again
    private Dictionary<PlayerLives, float> nextHitTime = new();

    void Reset()
    {
        // Ensure the collider is a trigger
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider other)
    {
        PlayerLives pl = other.GetComponentInParent<PlayerLives>();
        if (pl == null) return;

        // Cooldown check per player
        if (nextHitTime.TryGetValue(pl, out float allowedTime))
        {
            if (Time.time < allowedTime)
                return;
        }

        // Apply damage
        pl.TakeDamage(damage);

        // Set next allowed hit time
        nextHitTime[pl] = Time.time + hitCooldown;

        if (singleUse)
        {
            Destroy(gameObject);
        }
    }
}
