using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // LayerMask is added to help the 3D Raycast ignore the Player's own collider
    [Tooltip("Layers the player should consider as ground.")]
    public LayerMask groundLayer;

    public float speed;
    public float jump;
    public float groundedY;
    public float ledgeRay1;
    public float ledgeRay2;
    public float rayStart;
    public float ledgeRayLength;
    public Vector3 ledgeGrabTarget;
    public float ledgeGrabSpeed;

    bool controls = true;

    Animator animator;
    // Changed Rigidbody2D to Rigidbody
    new Rigidbody rigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // Changed GetComponent<Rigidbody2D>() to GetComponent<Rigidbody>()
        rigidbody = GetComponent<Rigidbody>();

        // Ensure Rigidbody constraints lock rotation on X and Z for 2D platforming feel
        if (rigidbody != null)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
    }

    void Update()
    {
        if (controls)
        {
            // Move on the X-axis (using horizontal input)
            // Use Vector3.right for 3D translation
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);

            CheckJump();

            CheckLedgeGrab();
        }

        CheckAnimations();
    }

    void CheckAnimations()
    {
        if (IsJumpFinished())
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                animator.Play("PlayerGoLeft");
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                animator.Play("PlayerGoRight");
            }
            else
            {
                animator.Play("Idle");
            }
        }
    }

    void CheckJump()
    {
        // Using KeyCode.Space for reliability, as recommended
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Use AddForce on the 3D Rigidbody
            // Use Vector3.up for 3D jump direction
            rigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);

            animator.Play("PlayerJump");

            if (Input.GetAxis("Horizontal") < 0) { animator.Play("PlayerJumpLeft"); }
        }
    }

    void CheckLedgeGrab()
    {
        float rayStartOriented = rayStart;

        // Changed Vector2 to Vector3
        Vector3 orientation = Vector3.right;

        Vector3 targetOriented = ledgeGrabTarget;

        if (Input.GetAxis("Horizontal") < 0)
        {
            rayStartOriented = -rayStart;
            orientation = -orientation;
            targetOriented.x = -targetOriented.x;
        }

        // Changed Physics2D.Raycast to Physics.Raycast and RaycastHit2D to RaycastHit
        RaycastHit hit1;
        if (Physics.Raycast(transform.position + new Vector3(rayStartOriented, ledgeRay1, 0), orientation, out hit1, ledgeRayLength))
        {
            // hit1.collider will be null if no hit occurs
            return;
        }

        RaycastHit hit2;
        if (!Physics.Raycast(transform.position + new Vector3(rayStartOriented, ledgeRay2, 0), orientation, out hit2, ledgeRayLength))
        {
            // If hit2 returns false (no collision), stop
            return;
        }

        if (!Input.GetKey(KeyCode.Space)) { return; }

        StartCoroutine(LedgeGrabRoutine(targetOriented));
    }

    public IEnumerator LedgeGrabRoutine(Vector3 targetPosition)
    {
        // Play your animation

        // 3D Rigidbody doesn't have linearVelocity, use velocity
        rigidbody.linearVelocity = Vector3.zero;

        // 3D Rigidbody doesn't have gravityScale, disabling the Rigidbody is one way to stop gravity
        // A better way is to set its IsKinematic flag, but let's stick closer to your original logic:
        rigidbody.useGravity = false;

        controls = false;

        Vector3 targetPositionWorld = transform.position + targetPosition;

        while (transform.position.y < targetPositionWorld.y)
        {
            transform.Translate(Vector3.up * Time.deltaTime * ledgeGrabSpeed); // Use Vector3.up
            yield return null;
        }

        while (targetPosition.x < 0 && transform.position.x > targetPositionWorld.x || targetPosition.x > 0 && transform.position.x < targetPositionWorld.x)
        {
            if (targetPosition.x <= 0) { transform.Translate(Vector3.left * Time.deltaTime * ledgeGrabSpeed); } // Use Vector3.left
            if (targetPosition.x > 0) { transform.Translate(Vector3.right * Time.deltaTime * ledgeGrabSpeed); } // Use Vector3.right
            yield return null;
        }

        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.useGravity = true; // Re-enable gravity
        controls = true;
    }

    public bool IsGrounded()
    {
        // Changed Physics2D.Raycast to Physics.Raycast and Vector2.down to Vector3.down
        // Added 'out hit' and 'groundLayer'
        RaycastHit hit;
        // The ray is cast from transform.position + offset, downwards, for 0.1 units, hitting only the groundLayer.
        if (Physics.Raycast(transform.position + new Vector3(0, groundedY, 0), Vector3.down, out hit, 0.2f, groundLayer))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsJumpFinished()
    {
        // Logic remains the same
        if (!IsGrounded()) { return false; }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) { return true; }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < animator.GetCurrentAnimatorStateInfo(0).length) { return false; }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Used Vector3.down for the 3D Ray Gizmo
        Gizmos.DrawRay(transform.position + new Vector3(0, groundedY, 0), Vector3.down * 0.1f);

        Gizmos.color = Color.green;
        // Used Vector3.right for the 3D Ray Gizmo
        Gizmos.DrawRay(transform.position + new Vector3(rayStart, ledgeRay1, 0), Vector3.right * ledgeRayLength);
        Gizmos.DrawRay(transform.position + new Vector3(rayStart, ledgeRay2, 0), Vector3.right * ledgeRayLength);
    }
}