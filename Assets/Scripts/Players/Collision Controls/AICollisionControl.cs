using UnityEngine;
using System.Collections;

public class AICollisionControl : PlayerControlBase
{
    private AIMovement aiMovement;
    private Collider _collider;

    public override void Start()
    {
        base.Start();
        aiMovement = GetComponent<AIMovement>();
        _collider = GetComponent<Collider>();
    }

    public override void HandleCollisionWithObstacle(Vector3 pushBackDirection)
    {
        transform.position += pushBackDirection * 0.5f;
        SetGhostLayer(true);

        if (aiMovement.isAlive)
            HandleDeathAndRespawn();
    }

    public override void HandleStickCollision(Vector3 explosionPosition)
    {
        aiMovement.SetAgentStatus();
        playerRb.AddExplosionForce(50f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);
        StartCoroutine(GetAgentBack());
    }

    private IEnumerator GetAgentBack()
    {
        yield return new WaitForSeconds(3f);
        if (aiMovement.isAlive) aiMovement.SetAgentStatus();
    }

    public override void HandleDeathAndRespawn()
    {
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
        SetGhostLayer(false);
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public override void HandleFinishLine()
    {
        if (aiMovement != null)
        {
            aiMovement.SetAgentStatus();
            aiMovement.AICharacterIsDone();
        }
    }

}
