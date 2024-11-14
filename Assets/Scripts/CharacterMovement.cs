using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance { get; private set; }

    public Joystick joystick;
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    public float rayDistance = 1.5f; 
    public LayerMask obstacleLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Ekledik
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Hareket yönünde Raycast gönder
        if (movement != Vector3.zero && !IsObstacleInPath(movement))
        {
            // Eðer yol boþsa hareket et
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animasyon ayarý
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }

    // Raycast ile çarpýþma kontrolü
    private bool IsObstacleInPath(Vector3 direction)
    {
        // Ray'i karakterin önüne doðru gönder
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, rayDistance, obstacleLayer))
        {
            // Eðer engel varsa true döner
            return true;
        }
        return false;
    }

    // Ray'i görünür kýlmak için
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }

}
