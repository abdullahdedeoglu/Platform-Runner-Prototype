using UnityEngine;
using System.Collections;

public class AICollisionControl : PlayerControlBase
{
    private AIMovement aiMovement; // Script that controls AI movement
    private Collider _collider;   // AI's collider component

    public override void Start()
    {
        base.Start();
        aiMovement = GetComponent<AIMovement>(); // Get the AI movement script
        _collider = GetComponent<Collider>();    // Get the collider reference
    }

    public override void HandleCollisionWithObstacle(Vector3 pushBackDirection)
    {
        // Push the AI character back
        transform.position += pushBackDirection * 0.5f;

        // Temporarily set the AI to the ghost layer
        SetGhostLayer(true);

        // If the AI is alive, handle its death and respawn
        if (aiMovement.isAlive)
            HandleDeathAndRespawn();
    }

    public override void HandleStickCollision(Vector3 explosionPosition)
    {
        // Disable the AI's movement
        aiMovement.SetAgentStatus();

        // Apply explosion force to the character
        playerRb.AddExplosionForce(50f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);

        // Reactivate the AI movement after a delay
        StartCoroutine(GetAgentBack());
    }

    private IEnumerator GetAgentBack()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        // Reactivate the AI movement if it is alive
        if (aiMovement.isAlive) aiMovement.SetAgentStatus();
    }

    public override void HandleDeathAndRespawn()
    {
        // Start the respawn process
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        animator.SetBool("isDead", true); // Trigger death animation

        // Stop AI movement
        if (aiMovement.agentActive) aiMovement.SetAgentStatus();
        aiMovement.ResetAICharacter(); // Reset AI character state

        yield return new WaitForSeconds(6f); // Wait for 6 seconds

        animator.SetBool("isDead", false); // Stop the death animation

        // Reset AI character state and reactivate movement
        aiMovement.ResetAICharacter();
        aiMovement.SetAgentStatus();

        // Reset AI position to the respawn point
        transform.position = respawnPosition;

        // Make the AI collidable again
        SetGhostLayer(false);

        // Reset any velocity or angular velocity
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public override void HandleFinishLine()
    {
        // If the AI movement script exists
        if (aiMovement != null)
        {
            aiMovement.SetAgentStatus(); // Stop AI movement
            aiMovement.AICharacterIsDone(); // Mark AI as finished
        }
    }
}
