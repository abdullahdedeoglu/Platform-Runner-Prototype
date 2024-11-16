using UnityEngine;
using System.Collections;

public class AICollisionControl : PlayerControlBase
{
    private AIMovement aiMovement;

    public override void Start()
    {
        base.Start();
        aiMovement = GetComponent<AIMovement>();
    }

    public override void HandleCollisionWithObstacle(Vector3 pushBackDirection)
    {
        transform.position += pushBackDirection * 0.5f;
        if (aiMovement.isAlive)
            HandleDeathAndRespawn();
    }

    public override void HandleStickCollision(Vector3 explosionPosition)
    {
        aiMovement.SetAgentStatus();
        playerRb.AddExplosionForce(500f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);
    }

    public override void HandleDeathAndRespawn()
    {
        Debug.Log("WHY GOD?");
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        animator.SetBool("isDead", true);

        // AI durdurulur
        if (aiMovement.agentActive) aiMovement.SetAgentStatus();
        aiMovement.ResetAICharacter();

        yield return new WaitForSeconds(6f);

        animator.SetBool("isDead", false);

        // AI yeniden baþlatýlýr
        aiMovement.ResetAICharacter();
        aiMovement.SetAgentStatus();

        transform.position = respawnPosition;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
}
