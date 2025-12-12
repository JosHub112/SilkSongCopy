using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerLifes : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Invulnerability")]
    public float invulDuration = 1.0f;
    public float blinkInterval = 0.12f;
    bool isInvulnerable = false;

    [Header("References")]
    public HealthUI ui; // drag HealthUI script (or the UI object) here
    public Animator animator;
    SpriteRenderer spriteRenderer;

    [Header("Optional")]
    public bool dieOnZero = true;

    void Awake()
    {
        currentHealth = maxHealth;
        if (animator == null) animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ui != null) ui.SetMaxHealth(maxHealth);
    }

    // Call this to apply damage from any source
    public void TakeDamage(int amount = 1)
    {
        if (isInvulnerable) return;
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        // UI + animation
        if (ui != null) ui.UpdateHearts(currentHealth);
        if (animator != null) animator.SetTrigger("Hit");

        // start invul period
        StartCoroutine(InvulnerabilityRoutine());

        if (currentHealth <= 0 && dieOnZero)
        {
            Die();
        }
    }

    void Die()
    {
        // play death animation and disable player controls here
        if (animator != null) animator.SetTrigger("Die");
        // optionally disable collider and/or set a 'dead' flag
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        // disable this script so no further damage
        enabled = false;
        // TODO: call respawn / game over logic from another manager
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulDuration)
        {
            // blink sprite
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2f;
        }
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    // Basic collision handling — works if enemy has Collider2D (isTrigger recommended)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // if enemy has EnemyDamage component, use its damage value
            EnemyDamage ed = other.GetComponent<EnemyDamage>();
            int dmg = ed != null ? ed.damage : 1;
            TakeDamage(dmg);
        }
    }

    // Alternative for non-trigger collisions:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyDamage ed = collision.collider.GetComponent<EnemyDamage>();
            int dmg = ed != null ? ed.damage : 1;
            TakeDamage(dmg);
        }
    }
}
