using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Joystick joystick;  // Reference to the on-screen joystick
    public float moveSpeed = 10f; // Movement speed

    private Rigidbody rb;  // Rigidbody for physics-based movement
    private Animator animator;  // Animator for controlling character animations

    public bool isAlive = true;  // Tracks if the character is active/alive

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth Rigidbody movements
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Get joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (isAlive)
        {
            // Move the character
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

            // Rotate the character to face the movement direction
            if (movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Update running animation based on movement
        animator.SetBool("isRunning", movement.magnitude > 0);
    }

    // Toggle the isAlive state
    public void SetIsAlive()
    {
        isAlive = !isAlive;
    }

    // Stop running animation explicitly
    public void StopRunning()
    {
        animator.SetBool("isRunning", false);
    }
}
