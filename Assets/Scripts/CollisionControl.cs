using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;  // Karakterin yeniden ba�layaca�� pozisyon
    public float stickForce = 20f;   // Stick �arp��mas�nda uygulanacak kuvvet
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Di�er engellere �arpt���nda yeniden ba�lat
            Respawn();
        }
        //else if (collision.gameObject.CompareTag("Rotator"))
        //{
        //    // Rotatora �arp�nca yeniden ba�lat
        //    Respawn();
        //}
        else if (collision.gameObject.CompareTag("Stick"))
        {
            // Stick ile �arp��mada karakteri �arpma y�n�ne g�re kuvvetle it
            ApplyStickForce(collision);
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

    private void ApplyStickForce(Collision collision)
    {
        // Stick �arpma y�n�n� hesapla ve kuvvet uygula
        Vector3 forceDirection = (transform.position - collision.transform.position).normalized;
        Vector3 force = forceDirection * stickForce;
        playerRb.AddForce(force, ForceMode.Impulse);

        // �arpma kuvvetini log'la
        Debug.Log("Stick Impact Force Applied: " + force);
        Debug.DrawRay(playerRb.position, force, Color.blue, 1f);
    }

    private void ApplyRotatingPlatformForce(Collision collision)
    {
        RotatingPlatform platform = collision.gameObject.GetComponent<RotatingPlatform>();
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

            Vector3 force = forceDirection * platform.rotationSpeed * platform.forceVelocity;
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
