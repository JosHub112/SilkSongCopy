using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lastInput;

    PlayerCore core;
    Animator anim;

    void Start()
    {
        core = GetComponent<PlayerCore>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        lastInput = input;

        float runspeed = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runspeed = 2f;
        }


            // Movement
            Vector3 speed = Vector3.right * input * moveSpeed * Time.deltaTime * runspeed;
        transform.Translate(speed);

        anim.SetFloat("Speed", Mathf.Abs(speed.x));
        Debug.Log("Speed: " + Mathf.Abs(speed.x));

        // Flip player model
        if (input != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(input); // 1 when moving right, -1 when left
            transform.localScale = scale;
        }

        HandleAnimations(input);
    }

    void HandleAnimations(float input)
    {
        if (!core.IsGrounded()) return;

        if (input != 0)
            core.anim.Play("PlayerRun"); // 1 animation for both directions
        else
            core.anim.Play("Idle");
    }
}
