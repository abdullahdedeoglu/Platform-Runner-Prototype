using UnityEngine;

public class RotatorMovement : MonoBehaviour
{
    public float rotationSpeed = 100f;  // Dönme hýzý

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
