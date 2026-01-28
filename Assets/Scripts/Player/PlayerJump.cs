using UnityEngine;

public class PlayerJump : PlayerMove
{
    public float jumpForce = 7f;
    [SerializeField] float extraJumpTime = 0.2f;
    private float _jumpTimeCounter;
    PlayerMove move;
    Animator animator;
    PlayerCore core;

    void Start()
    {
        animator = GetComponent<Animator>();
        core = GetComponent<PlayerCore>();
        move = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && core.IsGrounded())
        {
            animator.SetTrigger("Jump");
            core.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpTimeCounter = extraJumpTime;

        
                
        }

         if ( Input.GetKey(KeyCode.Space) && _jumpTimeCounter > 0)
           {
               core.rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
               _jumpTimeCounter -= Time.deltaTime;

           } 


    }
}
