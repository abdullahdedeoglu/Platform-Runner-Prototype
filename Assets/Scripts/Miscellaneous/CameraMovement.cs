using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public Transform player;
    private Vector3 offset = new Vector3(0, 10, -14);

    public GameObject cameraStartPoint;
    public GameObject paintingWallCameraPoint;

    public float duration = 1f;
    [SerializeField] private CameraMode currentMode = CameraMode.Follow;

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

    // Ölüm sonrasý geçiþi yumuþatmak için
    public IEnumerator SmoothTransitionTo(Vector3 targetPosition, Quaternion targetRotation, bool isNormal)
    {
        SetCameraMode(CameraMode.Transition);  // Geçiþ moduna geçiþ yap
        SoundManager.Instance.PlayCameraTransitionSound();

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

        //yield return new WaitForSeconds(4f);
        if(!isNormal)
            SetCameraMode(CameraMode.Follow);  // Geçiþ tamamlandýktan sonra takip moduna dön
        else
        {
            SetCameraMode(CameraMode.Normal); 
        }
    }
}

public enum CameraMode
{
    Follow,     // Karakteri takip etme modu
    Transition,
    Normal
}
