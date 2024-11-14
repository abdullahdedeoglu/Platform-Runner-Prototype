using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, -14);

    public float duration = 1f;
    private CameraMode currentMode = CameraMode.Follow;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (player == null || currentMode == CameraMode.Transition) return;

        // Kamera takip modunda ise oyuncuyu takip et
        if (currentMode == CameraMode.Follow)
        {
            transform.position = player.position + offset;
        }
    }

    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    // �l�m sonras� ge�i�i yumu�atmak i�in
    public IEnumerator SmoothTransitionTo(Vector3 targetPosition, Quaternion targetRotation)
    {
        SetCameraMode(CameraMode.Transition);  // Ge�i� moduna ge�i� yap
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        SetCameraMode(CameraMode.Follow);  // Ge�i� tamamland�ktan sonra takip moduna d�n
    }
}

public enum CameraMode
{
    Follow,     // Karakteri takip etme modu
    Transition  // Ge�i� modu (�l�m, yeniden do�ma gibi ge�i�ler)
}
