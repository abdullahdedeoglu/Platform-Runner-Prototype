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

    public bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Ekledik
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void FixedUpdate()
    {

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Hareket y�n�nde Raycast g�nder
        if (movement != Vector3.zero && !IsObstacleInPath(movement) && isAlive)
        {
            // E�er yol bo�sa hareket et
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animasyon ayar�
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }

    // Raycast ile �arp��ma kontrol�
    private bool IsObstacleInPath(Vector3 direction)
    {
        // Ray'i karakterin �n�ne do�ru g�nder
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, rayDistance, obstacleLayer))
        {
            // E�er engel varsa true d�ner
            return true;
        }
        return false;
    }

    // Ray'i g�r�n�r k�lmak i�in
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }

}
