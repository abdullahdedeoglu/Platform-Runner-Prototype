using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Joystick joystick;
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    public float forwardRayDistance = 1.5f; // Merkez ���n�n mesafesi
    public float sideRayDistance = 1f; // Yan ���nlar�n mesafesi
    public float sideRayOffset = 0.5f; // Sa� ve sol Ray'ler i�in pozisyon ofseti
    public LayerMask obstacleLayer; // Engeller i�in layer mask

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

        // �arp��ma durumuna g�re hareket et
        if (movement != Vector3.zero)
        {
            Vector3 adjustedMovement = GetAdjustedMovement(movement);
            rb.MovePosition(transform.position + adjustedMovement * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animasyon ayar�
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }

    // Hareket y�n�n� engellere g�re ayarla
    private Vector3 GetAdjustedMovement(Vector3 direction)
    {
        RaycastHit hit;

        Vector3 rightOffset = transform.right * sideRayOffset;
        Vector3 leftOffset = -transform.right * sideRayOffset;

        // Merkez ���n
        bool centerHit = Physics.Raycast(transform.position, direction, out hit, forwardRayDistance, obstacleLayer);
        // Sa� ���n
        bool rightHit = Physics.Raycast(transform.position + rightOffset, direction, out hit, sideRayDistance, obstacleLayer);
        // Sol ���n
        bool leftHit = Physics.Raycast(transform.position + leftOffset, direction, out hit, sideRayDistance, obstacleLayer);

        // �arp��ma durumlar�na g�re y�n ayar�
        if (centerHit && rightHit && leftHit)
        {
            // �� ���n da �arp���yorsa ileri hareketi engelle
            return Vector3.zero;
        }
        else if (centerHit && rightHit)
        {
            // Merkez ve sa� ���n �arp���yorsa sola kay
            return Quaternion.Euler(0, -45, 0) * direction;
        }
        else if (centerHit && leftHit)
        {
            // Merkez ve sol ���n �arp���yorsa sa�a kay
            return Quaternion.Euler(0, 45, 0) * direction;
        }
        else if (centerHit)
        {
            // Sadece merkez ���n �arp���yorsa ileri hareketi durdur
            return Vector3.zero;
        }

        // Hi�bir ���n �arp��mazsa orijinal y�nle hareket et
        return direction;
    }

    // Ray'leri g�rselle�tirme (iste�e ba�l�)
    private void OnDrawGizmos()
    {
        Vector3 rightOffset = transform.right * sideRayOffset;
        Vector3 leftOffset = -transform.right * sideRayOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * forwardRayDistance); // Merkez ���n
        Gizmos.DrawRay(transform.position + rightOffset, transform.forward * sideRayDistance); // Sa� ���n
        Gizmos.DrawRay(transform.position + leftOffset, transform.forward * sideRayDistance); // Sol ���n
    }
}
