using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;  // Karakterin yeniden ba�layaca�� 
    private Rigidbody playerRb;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Respawn();
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
            // Platformdaki hareketi zorla�t�rmak i�in kuvvet uygula
            ApplyRotatingPlatformForce(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            // Platformdan ayr�ld���nda h�zlar� s�f�rla
            ResetPlayerVelocity();
        }
    }

    private void Respawn()
    {
        // Karakteri ba�lang�� pozisyonuna d�nd�r ve h�z�n� s�f�rla
        transform.position = respawnPosition;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    private void ApplyStickForce(Collider other)
    {
        Vector3 explosionPosition = other.ClosestPoint(transform.position); // �arpma noktas�n� al
        float explosionForce = 50f;
        float explosionRadius = 2f;

        playerRb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 0.5f, ForceMode.Impulse);

        Debug.Log("Explosion Force Applied with Force: " + explosionForce + " | Radius: " + explosionRadius);
    }


    private void ApplyRotatingPlatformForce(Collision collision)
    {
        //RotatingPlatform platform = collision.gameObject.GetComponent<RotatingPlatform>();
        RotateAndMoveObstacle platform = collision.gameObject.GetComponent<RotateAndMoveObstacle>();
        //float rotationSpeed = 20f;
        float forceVelocity = 0.5f;

        if (platform != null)
        {
            // RotatingPlatform script'inden platformun d�n�� y�n�ne g�re kuvvet uygula
            Vector3 forceDirection;

            if (platform.clockwise)
            {
                // Saat y�n�nde d�n�yorsa -z y�n�nde 45 derece
                forceDirection = Quaternion.Euler(-45, -45, 0) * Vector3.forward;
            }
            else
            {
                // Saat y�n�n�n tersinde d�n�yorsa +z y�n�nde 45 derece
                forceDirection = Quaternion.Euler(45, 45, 0) * Vector3.forward;
            }

            Vector3 force = forceDirection * platform.rotationSpeed * forceVelocity;
            playerRb.AddForce(force, ForceMode.Acceleration);

            // Kuvveti log'la ve y�n�n� �iz
            Debug.Log("Rotating Platform Force Direction: " + forceDirection + " | Applied Force: " + force);
            Debug.DrawRay(playerRb.position, force, Color.red);
        }
    }

    private void ResetPlayerVelocity()
    {
        // Platformdan ayr�ld���nda h�zlar� s�f�rla
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
}
