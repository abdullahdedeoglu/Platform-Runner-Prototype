using UnityEngine;

public class ShiningObstacleMovement : MonoBehaviour
{
    public float moveDistance = 4f;
    public float speed = 2f;
    public float rotationSpeed = 50f;
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // X ekseninde ileri-geri hareket
        if (movingRight)
            transform.position += Vector3.right * speed * Time.deltaTime;
        else
            transform.position -= Vector3.right * speed * Time.deltaTime;

        // Hareket yönünü deðiþtirme
        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            movingRight = !movingRight;

        // Y ekseninde dönüþ
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
