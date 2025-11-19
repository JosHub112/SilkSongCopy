using UnityEngine;
using System.Collections;

public class PlayerLedgeGrab : MonoBehaviour
{
    [Header("Raycasts")]
    public float rayStartOffset = 0.5f;
    public float ray1Height = 1f;    // Upper ray: must NOT hit
    public float ray2Height = 0.5f;  // Lower ray: must hit
    public float rayLength = 0.6f;

    [Header("Climb Settings")]
    public Vector3 grabOffset;       // Where player ends up relative to start
    public float grabSpeed = 3f;

    PlayerCore core;
    bool controlsEnabled = true;

    void Start()
    {
        core = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if (!controlsEnabled) return;

        CheckForLedge();
    }

    void CheckForLedge()
    {
        float dir = GetDirectionInput(); // A = -1, D = +1

        if (dir == 0) return;                   // Not pressing left/right
        if (!Input.GetKey(KeyCode.Space)) return; // Must be holding jump (grab)

        Vector3 rayDirection = dir > 0 ? Vector3.right : Vector3.left;
        float offsetStart = dir > 0 ? rayStartOffset : -rayStartOffset;

        // Upper ray must NOT hit (space above the ledge)
        if (Physics.Raycast(transform.position + new Vector3(offsetStart, ray1Height, 0),
                            rayDirection, rayLength))
            return;

        // Lower ray MUST hit (detect the wall/ledge)
        if (!Physics.Raycast(transform.position + new Vector3(offsetStart, ray2Height, 0),
                             rayDirection, rayLength))
            return;

        // Start climbing
        StartCoroutine(LedgeClimbRoutine(dir));
    }

    IEnumerator LedgeClimbRoutine(float dir)
    {
        controlsEnabled = false;

        // Stop movement clean (rigidbody 3D!)
        core.rb.linearVelocity = Vector3.zero;
        core.rb.useGravity = false;

        Vector3 target = transform.position + new Vector3(grabOffset.x * dir, grabOffset.y, 0);

        // Climb UP
        while (transform.position.y < target.y)
        {
            Vector3 newPos = transform.position + Vector3.up * grabSpeed * Time.deltaTime;
            core.rb.MovePosition(newPos);
            yield return null;
        }

        // Climb SIDEWAYS
        while (Mathf.Abs(transform.position.x - target.x) > 0.05f)
        {
            Vector3 newPos = transform.position + Vector3.right * dir * grabSpeed * Time.deltaTime;
            core.rb.MovePosition(newPos);
            yield return null;
        }

        // Reset
        core.rb.useGravity = true;
        controlsEnabled = true;
    }

    private float GetDirectionInput()
    {
        if (Input.GetKey(KeyCode.A)) return -1f;
        if (Input.GetKey(KeyCode.D)) return 1f;
        return 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Visualize rays
        Gizmos.DrawRay(transform.position + new Vector3(rayStartOffset, ray1Height, 0),
                       Vector3.right * rayLength);

        Gizmos.DrawRay(transform.position + new Vector3(rayStartOffset, ray2Height, 0),
                       Vector3.right * rayLength);

        Gizmos.DrawRay(transform.position + new Vector3(-rayStartOffset, ray1Height, 0),
                       Vector3.left * rayLength);

        Gizmos.DrawRay(transform.position + new Vector3(-rayStartOffset, ray2Height, 0),
                       Vector3.left * rayLength);
    }
}
