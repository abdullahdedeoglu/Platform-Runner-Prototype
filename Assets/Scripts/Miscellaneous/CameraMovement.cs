using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance; // Singleton instance
    public Transform player;               // Reference to the player
    private Vector3 offset = new Vector3(0, 10, -14); // Offset for camera position

    public GameObject cameraStartPoint;        // Camera's start position
    public GameObject paintingWallCameraPoint; // Position for the painting wall camera

    public float duration = 1f; // Duration for smooth transitions
    [SerializeField] private CameraMode currentMode = CameraMode.Follow; // Current camera mode

    private void Awake()
    {
        // Ensure there is only one instance of CameraMovement
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (player == null || currentMode == CameraMode.Transition) return;

        // Follow the player in follow mode
        if (currentMode == CameraMode.Follow)
        {
            transform.position = player.position + offset;
        }
    }

    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    // Smoothly transitions the camera to a target position and rotation
    public IEnumerator SmoothTransitionTo(Vector3 targetPosition, Quaternion targetRotation, bool isNormal)
    {
        SetCameraMode(CameraMode.Transition); // Switch to transition mode
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

        if (!isNormal)
            SetCameraMode(CameraMode.Follow); // Return to follow mode after transition
        else
            SetCameraMode(CameraMode.Normal); // Switch to normal mode
    }
}

public enum CameraMode
{
    Follow,     // Follows the player
    Transition, // Smooth transition between points
    Normal      // Static or specific camera mode
}
