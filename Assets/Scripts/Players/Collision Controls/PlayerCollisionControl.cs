using UnityEngine;
using System.Collections;

public class PlayerCollisionControl : PlayerControlBase
{
    private CharacterMovement characterMovement;
    private Collider _collider;

    public override void Start()
    {
        base.Start();
        characterMovement = GetComponent<CharacterMovement>();
        _collider = GetComponent<Collider>();
    }

    public override void HandleCollisionWithObstacle(Vector3 pushBackDirection)
    {
        // Play the death sound effect
        SoundManager.Instance.PlayDeathSound();

        // Push the player back slightly
        transform.position += pushBackDirection * 0.5f;

        // Set the ghost layer for collision handling
        SetGhostLayer(true);

        // Handle death and respawn if the player is alive
        if (characterMovement.isAlive)
            HandleDeathAndRespawn();
    }

    public override void HandleStickCollision(Vector3 explosionPosition)
    {
        // Apply an explosion force to the player's rigidbody
        playerRb.AddExplosionForce(25f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);
    }

    public override void HandleDeathAndRespawn()
    {
        // Increment the death counter in the GameManager
        GameManager.Instance.UpgradeDeathAmount();

        // Start the respawn coroutine
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // Set the "isDead" animation state to true
        animator.SetBool("isDead", true);

        // Kill the player
        characterMovement.SetIsAlive();

        // Wait for 4 seconds
        yield return new WaitForSeconds(4f);

        // Reset the "isDead" animation state
        animator.SetBool("isDead", false);

        // Smooth camera transition
        yield return StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(
            CameraMovement.Instance.cameraStartPoint.transform.position,
            CameraMovement.Instance.cameraStartPoint.transform.rotation,
            false));

        // Respawn the player
        characterMovement.SetIsAlive();
        transform.position = respawnPosition;

        // Reset the ghost layer
        SetGhostLayer(false);

        // Reset the player's velocity
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public override void HandleFinishLine()
    {
        // Stop the character's running animation
        characterMovement.StopRunning();

        // Set a target position for the finish
        Vector3 positionToSetTarget = new Vector3(-2.5f, 0, 220f);

        // Start a coroutine to move to the target position over 2 seconds
        StartCoroutine(MoveToTarget(transform.position, positionToSetTarget, 2f));

        // Disable the character's movement
        characterMovement.enabled = false;

        // Trigger additional processes if needed (e.g., animation, camera transitions)
        GameManager.Instance.OnPlayerFinish();
    }

    private IEnumerator MoveToTarget(Vector3 start, Vector3 target, float duration)
    {
        float elapsed = 0f;

        // Gradually move the object towards the target over the given duration
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Snap to the target position
        transform.position = target;
    }
}
