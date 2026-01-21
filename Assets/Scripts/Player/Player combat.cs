using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Attack Settings")]
    public int attackDamage = 40;
    public float attackRange = 0.5f;

    [Header("Timing (match your animation)")]
    public float attackHitDelay = 0.3f;   // when damage happens
    public float attackTotalTime = 0.7f;  // full animation length

    public bool IsAttacking { get; private set; }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // Prevent attack spam
            if (!IsAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        IsAttacking = true;

        // Trigger attack animation
        animator.SetTrigger("Attack");

        // Wait until the hit frame
        yield return new WaitForSeconds(attackHitDelay);

        // Detect enemies
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        // Wait for the rest of the animation
        yield return new WaitForSeconds(attackTotalTime - attackHitDelay);

        IsAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
