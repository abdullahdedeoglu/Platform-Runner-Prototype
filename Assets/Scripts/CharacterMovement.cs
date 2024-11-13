using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Joystick joystick;
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    public float forwardRayDistance = 1.5f; // Merkez ýþýnýn mesafesi
    public float sideRayDistance = 1f; // Yan ýþýnlarýn mesafesi
    public float sideRayOffset = 0.5f; // Sað ve sol Ray'ler için pozisyon ofseti
    public LayerMask obstacleLayer; // Engeller için layer mask

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

        // Çarpýþma durumuna göre hareket et
        if (movement != Vector3.zero)
        {
            Vector3 adjustedMovement = GetAdjustedMovement(movement);
            rb.MovePosition(transform.position + adjustedMovement * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(adjustedMovement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Animasyon ayarý
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRunning", isRunning);
    }

    // Hareket yönünü engellere göre ayarla
    private Vector3 GetAdjustedMovement(Vector3 direction)
    {
        RaycastHit hit;

        Vector3 rightOffset = transform.right * sideRayOffset;
        Vector3 leftOffset = -transform.right * sideRayOffset;

        // Merkez ýþýn
        bool centerHit = Physics.Raycast(transform.position, direction, out hit, forwardRayDistance, obstacleLayer);
        // Sað ýþýn
        bool rightHit = Physics.Raycast(transform.position + rightOffset, direction, out hit, sideRayDistance, obstacleLayer);
        // Sol ýþýn
        bool leftHit = Physics.Raycast(transform.position + leftOffset, direction, out hit, sideRayDistance, obstacleLayer);

        // Çarpýþma durumlarýna göre yön ayarý
        if (centerHit && rightHit && leftHit)
        {
            // Üç ýþýn da çarpýþýyorsa ileri hareketi engelle
            return Vector3.zero;
        }
        else if (centerHit && rightHit)
        {
            // Merkez ve sað ýþýn çarpýþýyorsa sola kay
            return Quaternion.Euler(0, -45, 0) * direction;
        }
        else if (centerHit && leftHit)
        {
            // Merkez ve sol ýþýn çarpýþýyorsa saða kay
            return Quaternion.Euler(0, 45, 0) * direction;
        }
        else if (centerHit)
        {
            // Sadece merkez ýþýn çarpýþýyorsa ileri hareketi durdur
            return Vector3.zero;
        }

        // Hiçbir ýþýn çarpýþmazsa orijinal yönle hareket et
        return direction;
    }

    // Ray'leri görselleþtirme (isteðe baðlý)
    private void OnDrawGizmos()
    {
        Vector3 rightOffset = transform.right * sideRayOffset;
        Vector3 leftOffset = -transform.right * sideRayOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * forwardRayDistance); // Merkez ýþýn
        Gizmos.DrawRay(transform.position + rightOffset, transform.forward * sideRayDistance); // Sað ýþýn
        Gizmos.DrawRay(transform.position + leftOffset, transform.forward * sideRayDistance); // Sol ýþýn
    }
}
