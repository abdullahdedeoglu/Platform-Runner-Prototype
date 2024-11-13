using UnityEngine;

public class RotatorMovement : MonoBehaviour
{
    public float rotationSpeed = 100f;  // D�nme h�z�

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
