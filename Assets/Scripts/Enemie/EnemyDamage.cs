using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public bool onlyDamageOncePerContact = true;

    // simple cooldown to avoid spamming if you keep touching (optional)
    public float rehitCooldown = 0.5f;
    float lastHitTime = -10f;

    private void Reset()
    {
        // ensure collider is trigger by default for simple use
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - lastHitTime < rehitCooldown) return;

        if (other.CompareTag("Player"))
        {
            // PlayerHealth will handle invul, so we can just call it
            var ph = other.GetComponent<PlayerLifes>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                lastHitTime = Time.time;
            }
        }
    }

    // Draw a gizmo so you can see the block while editing
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        // Draw the collider bounds if possible
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            Gizmos.DrawCube(b.center, b.size);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(b.center, b.size);
        }
        else
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}
