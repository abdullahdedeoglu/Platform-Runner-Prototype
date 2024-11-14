using UnityEngine;
using System.Collections;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;
    public Animator animator;

    private Rigidbody playerRb;
    [SerializeField] private GameObject cameraStartPoint;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Vector3 pushBackDirection = (transform.position - other.transform.position).normalized;
            transform.position += pushBackDirection * 0.5f;
            if (CharacterMovement.Instance.isAlive == true) 
                StartCoroutine(HandleDeathAndRespawn());
        }
        else if (other.CompareTag("Stick"))
        {
            ApplyStickForce(other);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            // Platformdaki hareketi zorlaþtýrmak için kuvvet uygula
            ApplyRotatingPlatformForce(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            // Platformdan ayrýldýðýnda hýzlarý sýfýrla
            ResetPlayerVelocity();
        }
    }

    private IEnumerator HandleDeathAndRespawn()
    {
        animator.SetBool("isDead", true);
        CharacterMovement.Instance.isAlive = false;
        yield return new WaitForSeconds(4f);
        animator.SetBool("isDead", false);

        // Kamera geçiþini baþlat
        yield return StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(cameraStartPoint.transform.position, cameraStartPoint.transform.rotation));

        // Karakteri yeniden baþlatma konumuna taþý
        CharacterMovement.Instance.isAlive = true;
        transform.position = respawnPosition;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    private void ApplyStickForce(Collider other)
    {
        Vector3 explosionPosition = other.ClosestPoint(transform.position);
        playerRb.AddExplosionForce(50f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);
    }

    private void ApplyRotatingPlatformForce(Collision collision)
    {
        RotateAndMoveObstacle platform = collision.gameObject.GetComponent<RotateAndMoveObstacle>();
        float forceVelocity = 0.5f;
        if (platform != null)
        {
            Vector3 forceDirection = platform.clockwise ? Quaternion.Euler(-45, -45, 0) * Vector3.forward
                                                         : Quaternion.Euler(45, 45, 0) * Vector3.forward;
            Vector3 force = forceDirection * platform.rotationSpeed * forceVelocity;
            playerRb.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void ResetPlayerVelocity()
    {
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
}
