using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    PlayerCore core;

    void Start()
    {
        core = GetComponent<PlayerCore>();
    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);

        HandleAnimations(input);
    }

    void HandleAnimations(float input)
    {
        if (!core.IsGrounded()) return;

        if (input < 0) core.anim.Play("PlayerGoLeft");
        else if (input > 0) core.anim.Play("PlayerGoRight");
        else core.anim.Play("Idle");
    }
}
