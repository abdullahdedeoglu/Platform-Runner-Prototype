using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public Joystick joystick;
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;

    public bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (isAlive)
        {
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animasyon ayarý
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }

    public void SetIsAlive()
    {
        isAlive = !isAlive;
    }

    public void StopRunning()
    {
        animator.SetBool("isRunning", false);
    }

}
