using UnityEngine;

public abstract class PlayerControlBase : MonoBehaviour
{
    public Vector3 respawnPosition;
    protected Animator animator;
    protected Rigidbody playerRb;

    public virtual void Start()
    {
        // Get Rigidbody and Animator components
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Check if Animator is assigned
        if (animator == null)
            Debug.LogError("Animator is not assigned!");

        // Set the initial position as the respawn point
        respawnPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision with obstacles
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 pushBackDirection = (transform.position - collision.transform.position).normalized;
            SoundManager.Instance.PlayCollisionSound();
            HandleCollisionWithObstacle(pushBackDirection);
        }
        // Handle collision with sticks
        else if (collision.gameObject.CompareTag("Stick"))
        {
            Vector3 explosionPosition = collision.contacts[0].point;
            SoundManager.Instance.PlayCollisionSound();
            HandleStickCollision(explosionPosition);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Apply force when the player is on a rotating platform
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            ApplyRotatingPlatformForce(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset player velocity when leaving a rotating platform
        if (collision.gameObject.CompareTag("RotatingPlatform"))
        {
            ResetPlayerVelocity();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle death trigger
        if (other.CompareTag("DeathTrigger"))
        {
            Vector3 pushBackDirection = Vector3.zero; // No specific direction for death trigger
            HandleCollisionWithObstacle(pushBackDirection);
        }
        // Handle finish line trigger
        if (other.CompareTag("FinishLine"))
        {
            HandleFinishLine();
        }
    }

    private void ApplyRotatingPlatformForce(Collision collision)
    {
        // Apply force based on the rotating platform's direction and speed
        RotateAndMoveObstacle platform = collision.gameObject.GetComponent<RotateAndMoveObstacle>();
        if (platform != null)
        {
            Vector3 forceDirection = platform.clockwise
                ? Quaternion.Euler(-45, -45, 0) * Vector3.forward
                : Quaternion.Euler(45, 45, 0) * Vector3.forward;

            Vector3 force = forceDirection * platform.rotationSpeed * 0.5f;
            playerRb.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void ResetPlayerVelocity()
    {
        // Reset the player's velocity and angular velocity
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public void SetGhostLayer(bool isGhost)
    {
        // Set the layer to "Ghost" or "Player" based on the given state
        gameObject.layer = isGhost ? LayerMask.NameToLayer("Ghost") : LayerMask.NameToLayer("Player");
    }

    // Abstract methods to be implemented by derived classes
    public abstract void HandleCollisionWithObstacle(Vector3 pushBackDirection);
    public abstract void HandleStickCollision(Vector3 explosionPosition);
    public abstract void HandleDeathAndRespawn();
    public abstract void HandleFinishLine();
}
