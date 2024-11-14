using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 20f; // Platformun dönüþ hýzý
    public bool clockwise = true;     // Saat yönünde dönüþ
    public float forceVelocity = 1f;

    private void FixedUpdate()
    {
        // Platformu z ekseni etrafýnda döndür
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
    //            // Sabit bir açý ile kuvvet uygulama
    //            Vector3 forceDirection;

    //            if (clockwise)
    //            {
    //                // Saat yönünde dönüyorsa, -z yönünde 45 derecelik açý
    //                forceDirection = Quaternion.Euler(-45, -45, 0) * Vector3.forward;
    //            }
    //            else
    //            {
    //                // Saat yönünün tersinde dönüyorsa, +z yönünde 45 derecelik açý
    //                forceDirection = Quaternion.Euler(45, 45, 0) * Vector3.forward;
    //            }

    //            Vector3 force = forceDirection * rotationSpeed * forceVelocity;

    //            // Kuvveti uygula
    //            playerRb.AddForce(force, ForceMode.Acceleration);

    //            // Kuvveti log'la
    //            Debug.Log("Applied Force Direction: " + forceDirection + " | Applied Force: " + force);

    //            // Kuvvetin yönünü sahnede çiz
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
    //            // Platformdan ayrýldýðýnda oyuncunun hýzýný sýfýrla
    //            playerRb.velocity = Vector3.zero;
    //            playerRb.angularVelocity = Vector3.zero;
    //        }
    //    }
    //}
}
