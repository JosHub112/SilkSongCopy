using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamageTrigger : MonoBehaviour
{
    public int damage = 1;
    public bool singleUse = false;

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c != null)
        {
            c.isTrigger = true;
            Debug.Log("[EnemyDamageTrigger] Reset: Collider set to Trigger");
        }
    }

    void Awake()
    {
        Debug.Log("[EnemyDamageTrigger] Awake on: " + gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[EnemyDamageTrigger] Trigger ENTER detected on: " + gameObject.name);
        Debug.Log("[EnemyDamageTrigger] Collided with: " + other.gameObject.name);

        var pl = other.GetComponent<PlayerLives>();

        if (pl == null)
        {
            Debug.Log("[EnemyDamageTrigger] PlayerLives NOT found on object");
            pl = other.GetComponentInParent<PlayerLives>();
        }

        if (pl != null)
        {
            Debug.Log("[EnemyDamageTrigger] PlayerLives FOUND, applying damage: " + damage);
            pl.TakeDamage(damage);

            if (singleUse)
            {
                Debug.Log("[EnemyDamageTrigger] Single use enabled, destroying: " + gameObject.name);
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("[EnemyDamageTrigger] No valid PlayerLives script found anywhere");
        }
    }
}
