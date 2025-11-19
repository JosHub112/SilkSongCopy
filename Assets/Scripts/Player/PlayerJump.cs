using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 7f;

    PlayerCore core;

    void Start()
    {
        core = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && core.IsGrounded())
        {
            core.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            core.anim.Play("PlayerJump");
        }
    }
}
