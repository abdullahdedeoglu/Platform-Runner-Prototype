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
        SoundManager.Instance.PlayDeathSound();
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
        GameManager.Instance.UpgradeDeathAmount();
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
            CameraMovement.Instance.cameraStartPoint.transform.rotation,
            false));

        // Yeniden doðma
        characterMovement.SetIsAlive();
        transform.position = respawnPosition;
        SetGhostLayer(false);
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

    public override void HandleFinishLine()
    {
        characterMovement.StopRunning(); // Karakterin koþma animasyonunu durdur

        Vector3 positionToSetTarget = new Vector3(-2.5f, 0, 220f);

        // Coroutine baþlatýlarak hareket zamana yayýlýr
        StartCoroutine(MoveToTarget(transform.position, positionToSetTarget, 2f)); // 2 saniyede hedefe ulaþ

        characterMovement.enabled = false;

        // Gerekirse diðer iþlemleri tetikle (animasyon, kamera geçiþi vb.)
        GameManager.Instance.OnPlayerFinish(); // Örnek bir GameManager fonksiyonu
    }

    private IEnumerator MoveToTarget(Vector3 start, Vector3 target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration); // Hareketi zamana yay
            elapsed += Time.deltaTime; // Geçen zamaný artýr
            yield return null; // Bir sonraki frame'i bekle
        }

        transform.position = target; // Hedef konuma tam olarak yerleþ
    }



}
