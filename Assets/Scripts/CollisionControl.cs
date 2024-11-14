using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;  // Karakterin yeniden baþlayacaðý 
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

    private void Respawn()
    {
        // Karakteri baþlangýç pozisyonuna döndür ve hýzýný sýfýrla
        transform.position = respawnPosition;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    private void ApplyStickForce(Collider other)
    {
        Vector3 explosionPosition = other.ClosestPoint(transform.position); // Çarpma noktasýný al
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
            // RotatingPlatform script'inden platformun dönüþ yönüne göre kuvvet uygula
            Vector3 forceDirection;

            if (platform.clockwise)
            {
                // Saat yönünde dönüyorsa -z yönünde 45 derece
                forceDirection = Quaternion.Euler(-45, -45, 0) * Vector3.forward;
            }
            else
            {
                // Saat yönünün tersinde dönüyorsa +z yönünde 45 derece
                forceDirection = Quaternion.Euler(45, 45, 0) * Vector3.forward;
            }

            Vector3 force = forceDirection * platform.rotationSpeed * forceVelocity;
            playerRb.AddForce(force, ForceMode.Acceleration);

            // Kuvveti log'la ve yönünü çiz
            Debug.Log("Rotating Platform Force Direction: " + forceDirection + " | Applied Force: " + force);
            Debug.DrawRay(playerRb.position, force, Color.red);
        }
    }

    private void ResetPlayerVelocity()
    {
        // Platformdan ayrýldýðýnda hýzlarý sýfýrla
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
}
