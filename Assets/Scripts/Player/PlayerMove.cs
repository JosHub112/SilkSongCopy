using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Animator animator;

    public float lastInput { get; private set; }

    PlayerCombat combat;

    void Start()
    {
        combat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        lastInput = input;

        // Move
        transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);

        // Flip player (direction change does NOT stop run)
        if (input != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(input);
            transform.localScale = scale;
        }

        // Animator (run if moving, regardless of direction)
        animator.SetBool("Isrunning", input != 0);
    }
}
