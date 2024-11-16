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
        transform.position += pushBackDirection * 0.5f;
        SetGhostLayer(true);
        if (characterMovement.isAlive)
            HandleDeathAndRespawn();
    }

    public override void HandleStickCollision(Vector3 explosionPosition)
    {
        playerRb.AddExplosionForce(25f, explosionPosition, 2f, 0.5f, ForceMode.Impulse);
    }

    public override void HandleDeathAndRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        animator.SetBool("isDead", true);
        characterMovement.SetIsAlive(); // Oyuncuyu öldür
        yield return new WaitForSeconds(4f);
        animator.SetBool("isDead", false);

        // Kamera geçiþi
        yield return StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(
            CameraMovement.Instance.cameraStartPoint.transform.position,
            CameraMovement.Instance.cameraStartPoint.transform.rotation));

        // Yeniden doðma
        characterMovement.SetIsAlive();
        transform.position = respawnPosition;
        SetGhostLayer(false);
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }



}
