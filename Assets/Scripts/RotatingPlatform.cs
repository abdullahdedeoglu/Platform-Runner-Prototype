using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 20f; // Platformun d�n�� h�z�
    public bool clockwise = true;     // Saat y�n�nde d�n��
    public float forceVelocity = 1f;

    private void FixedUpdate()
    {
        // Platformu z ekseni etraf�nda d�nd�r
        float direction = clockwise ? 1f : -1f;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

    //        if (playerRb != null)
    //        {
    //            // Sabit bir a�� ile kuvvet uygulama
    //            Vector3 forceDirection;

    //            if (clockwise)
    //            {
    //                // Saat y�n�nde d�n�yorsa, -z y�n�nde 45 derecelik a��
    //                forceDirection = Quaternion.Euler(-45, -45, 0) * Vector3.forward;
    //            }
    //            else
    //            {
    //                // Saat y�n�n�n tersinde d�n�yorsa, +z y�n�nde 45 derecelik a��
    //                forceDirection = Quaternion.Euler(45, 45, 0) * Vector3.forward;
    //            }

    //            Vector3 force = forceDirection * rotationSpeed * forceVelocity;

    //            // Kuvveti uygula
    //            playerRb.AddForce(force, ForceMode.Acceleration);

    //            // Kuvveti log'la
    //            Debug.Log("Applied Force Direction: " + forceDirection + " | Applied Force: " + force);

    //            // Kuvvetin y�n�n� sahnede �iz
    //            Debug.DrawRay(playerRb.position, force, Color.red);
    //        }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
    //        if (playerRb != null)
    //        {
    //            // Platformdan ayr�ld���nda oyuncunun h�z�n� s�f�rla
    //            playerRb.velocity = Vector3.zero;
    //            playerRb.angularVelocity = Vector3.zero;
    //        }
    //    }
    //}
}
