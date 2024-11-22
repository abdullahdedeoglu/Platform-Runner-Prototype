using UnityEngine;
using System.Collections;

public class RotateAndMoveObstacle : MonoBehaviour
{
    // Enumeration for rotation axis selection
    public enum RotationAxis { Y, Z }

    [HideInInspector] public bool rotate = true; // Is rotation enabled?
    [HideInInspector] public bool clockwise = false; // Should it rotate clockwise?
    [HideInInspector] public float rotationSpeed = 20f; // Speed of rotation
    [HideInInspector] public RotationAxis rotationAxis = RotationAxis.Y; // Selected rotation axis (Y or Z)

    [HideInInspector] public bool move = true; // Is movement enabled?
    [HideInInspector] public bool pauseBetweenMoves = false; // Should there be pauses between movements?
    [HideInInspector] public float moveDistance = 4f; // Distance to move before changing direction
    [HideInInspector] public float moveSpeed = 2f; // Speed of movement
    [HideInInspector] public float pauseDuration = 1f; // Duration of pause between movements

    private Vector3 startPosition; // Initial position of the obstacle
    private bool movingRight = true; // Direction of movement (true = right, false = left)

    void Start()
    {
        // Store the starting position of the obstacle
        startPosition = transform.position;

        // If movement is enabled and pauses are required, start the coroutine for movement with pauses
        if (move && pauseBetweenMoves)
        {
            StartCoroutine(MoveWithPause());
        }
    }

    void Update()
    {
        // Handle rotation if enabled
        if (rotate)
        {
            float direction = clockwise ? 1f : -1f; // Set rotation direction based on the clockwise flag
            Vector3 rotationAxisVector = (rotationAxis == RotationAxis.Y) ? Vector3.up : Vector3.forward; // Determine the axis of rotation
            transform.Rotate(rotationAxisVector * direction * rotationSpeed * Time.deltaTime); // Apply rotation
        }

        // Handle continuous movement if enabled and pauses are not required
        if (move && !pauseBetweenMoves)
        {
            MoveContinuously();
        }
    }

    private void MoveContinuously()
    {
        // Move in the current direction
        if (movingRight)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        else
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime;

        // Check if the obstacle has reached the movement limit
        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            movingRight = !movingRight; // Reverse direction
    }

    private IEnumerator MoveWithPause()
    {
        while (true) // Infinite loop to handle movement
        {
            if (movingRight)
            {
                // Move to the right until the movement distance is reached
                while (Vector3.Distance(startPosition, transform.position) < moveDistance)
                {
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
            }
            else
            {
                // Move to the left until the movement distance is reached
                while (Vector3.Distance(startPosition, transform.position) < moveDistance)
                {
                    transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
            }

            yield return new WaitForSeconds(pauseDuration); // Wait for the pause duration
            startPosition = transform.position; // Update the starting position
            movingRight = !movingRight; // Reverse direction
        }
    }
}
