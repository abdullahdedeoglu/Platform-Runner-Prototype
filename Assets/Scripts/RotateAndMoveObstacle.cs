using UnityEngine;
using System.Collections;

public class RotateAndMoveObstacle : MonoBehaviour
{
    public enum RotationAxis { Y, Z } // Dönme ekseni seçeneði

    [HideInInspector] public bool rotate = true; // Dönme özelliði aktif 
    [HideInInspector] public bool clockwise = false;            // Saat yönünde mi dönsün
    [HideInInspector] public float rotationSpeed = 20f;        // Dönüþ hýzý
    [HideInInspector] public RotationAxis rotationAxis = RotationAxis.Y; // Dönme ekseni seçimi

    [HideInInspector] public bool move = true;                 // Hareket özelliði aktif mi
    [HideInInspector] public bool pauseBetweenMoves = false;   // Hareketlerde duraklama olacak mý
    [HideInInspector] public float moveDistance = 4f;          // Hareket mesafesi
    [HideInInspector] public float moveSpeed = 2f;             // Hareket hýzý
    [HideInInspector] public float pauseDuration = 1f;         // Duraklama süresi

    private Vector3 startPosition;           // Baþlangýç pozisyonu
    private bool movingRight = true;         // Hareket yönü

    void Start()
    {
        startPosition = transform.position;

        if (move && pauseBetweenMoves)
        {
            StartCoroutine(MoveWithPause());
        }
    }

    void Update()
    {
        if (rotate)
        {
            float direction = clockwise ? 1f : -1f;
            Vector3 rotationAxisVector = (rotationAxis == RotationAxis.Y) ? Vector3.up : Vector3.forward;
            transform.Rotate(rotationAxisVector * direction * rotationSpeed * Time.deltaTime);
        }

        if (move && !pauseBetweenMoves)
        {
            MoveContinuously();
        }
    }

    private void MoveContinuously()
    {
        if (movingRight)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        else
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            movingRight = !movingRight;
    }

    private IEnumerator MoveWithPause()
    {
        while (true)
        {
            if (movingRight)
            {
                while (Vector3.Distance(startPosition, transform.position) < moveDistance)
                {
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (Vector3.Distance(startPosition, transform.position) < moveDistance)
                {
                    transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(pauseDuration);
            startPosition = transform.position;
            movingRight = !movingRight;
        }
    }



}


