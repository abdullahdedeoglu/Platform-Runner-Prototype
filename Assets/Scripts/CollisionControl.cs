using UnityEngine;
using System.Collections;

public class CollisionControl : MonoBehaviour
{
    public Vector3 respawnPosition;
    public Animator animator;

    private Rigidbody playerRb;
    [SerializeField] private GameObject cameraStartPoint;
    [SerializeField] private bool isPlayer = false;

    private AIMovement aiMovement;
    private CharacterMovement characterMovement;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        if (!isPlayer) aiMovement = GetComponent<AIMovement>();
        else characterMovement = GetComponent<CharacterMovement>();


        if (animator == null)
            animator = GetComponent<Animator>();

        respawnPosition = this.gameObject.transform.position;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Vector3 pushBackDirection = (transform.position - other.transform.position).normalized;
            transform.position += pushBackDirection * 0.5f;

            if (isPlayer)
            {
                if (characterMovement.isAlive)
                {
                    StartCoroutine(HandleDeathAndRespawn());
                }
            }
            else
            {
                if(aiMovement.isAlive)
                {
                    StartCoroutine(HandleDeathAndRespawn());
                }

            }

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

        if (isPlayer)
        {
            characterMovement.SetIsAlive();
            yield return new WaitForSeconds(4f);
        }
        else
        {
            aiMovement.ResetAICharacter();
            yield return new WaitForSeconds(3f);
        }
        
        animator.SetBool("isDead", false);

        // Kamera geçiþini baþlat
        if (isPlayer)
            yield return StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(cameraStartPoint.transform.position, cameraStartPoint.transform.rotation));

        // Karakteri yeniden baþlatma konumuna taþý
        if (isPlayer)
            characterMovement.SetIsAlive();
        else
            aiMovement.ResetAICharacter();

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
