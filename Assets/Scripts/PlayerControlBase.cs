using UnityEngine;

public abstract class PlayerControlBase : MonoBehaviour
{
    public Vector3 respawnPosition;
    protected Animator animator;
    protected Rigidbody playerRb;

    public virtual void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogError("Animator atanmadý!");

        respawnPosition = transform.position; // Ýlk konumu respawn noktasý olarak belirle
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 pushBackDirection = (transform.position - collision.transform.position).normalized;
            HandleCollisionWithObstacle(pushBackDirection);
        }
        else if (collision.gameObject.CompareTag("Stick"))
        {
            Vector3 explosionPosition = collision.contacts[0].point;
            HandleStickCollision(explosionPosition);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            ApplyRotatingPlatformForce(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            ResetPlayerVelocity();
        }
    }

    private void ApplyRotatingPlatformForce(Collision collision)
    {
        RotateAndMoveObstacle platform = collision.gameObject.GetComponent<RotateAndMoveObstacle>();
        if (platform != null)
        {
            Vector3 forceDirection = platform.clockwise ? Quaternion.Euler(-45, -45, 0) * Vector3.forward
                                                         : Quaternion.Euler(45, 45, 0) * Vector3.forward;
            Vector3 force = forceDirection * platform.rotationSpeed * 0.5f;
            playerRb.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void ResetPlayerVelocity()
    {
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public abstract void HandleCollisionWithObstacle(Vector3 pushBackDirection);
    public abstract void HandleStickCollision(Vector3 explosionPosition);
    public abstract void HandleDeathAndRespawn();
}
