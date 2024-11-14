using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance { get; private set; }

    public Joystick joystick;
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    public float rayDistance = 1.5f; // �arp��malar� �nceden alg�lamak i�in ray mesafesi
    public LayerMask obstacleLayer;  // Sadece engellerle �arp��may� sa�lamak i�in layer ayar�
    //public bool onRotatePlatform = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Hareket y�n�nde Raycast g�nder
        if (movement != Vector3.zero && !IsObstacleInPath(movement))
        {
            // E�er yol bo�sa hareket et
            rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        //if (onRotatePlatform) ApplyRotationForce(movement);

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

    //public void ApplyRotationForce(Vector3 movement)
    //{
    //    float rotatitonSpeed = 3f;

    //    Vector3 forceDirection = transform.right * rotatitonSpeed;

    //    if (movement.magnitude > 0)
    //    {
    //        rb.AddForce(-forceDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
    //    }
    //}
}
