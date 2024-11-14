using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;  // Karakterin yeniden baþlayacaðý pozisyon
    public float stickForce = 20f;   // Stick çarpýþmasýnda uygulanacak kuvvet
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Diðer engellere çarptýðýnda yeniden baþlat
            Respawn();
        }
        //else if (collision.gameObject.CompareTag("Rotator"))
        //{
        //    // Rotatora çarpýnca yeniden baþlat
        //    Respawn();
        //}
        else if (collision.gameObject.CompareTag("Stick"))
        {
            // Stick ile çarpýþmada karakteri çarpma yönüne göre kuvvetle it
            ApplyStickForce(collision);
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

    private void ApplyStickForce(Collision collision)
    {
        // Stick çarpma yönünü hesapla ve kuvvet uygula
        Vector3 forceDirection = (transform.position - collision.transform.position).normalized;
        Vector3 force = forceDirection * stickForce;
        playerRb.AddForce(force, ForceMode.Impulse);

        // Çarpma kuvvetini log'la
        Debug.Log("Stick Impact Force Applied: " + force);
        Debug.DrawRay(playerRb.position, force, Color.blue, 1f);
    }

    private void ApplyRotatingPlatformForce(Collision collision)
    {
        RotatingPlatform platform = collision.gameObject.GetComponent<RotatingPlatform>();
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

            Vector3 force = forceDirection * platform.rotationSpeed * platform.forceVelocity;
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
