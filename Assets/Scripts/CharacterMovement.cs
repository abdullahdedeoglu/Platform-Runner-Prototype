using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Joystick joystick;
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }
}