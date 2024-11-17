using UnityEngine;
using System.Collections;

public class RotateAndMoveObstacle : MonoBehaviour
{
    public enum RotationAxis { Y, Z } // D�nme ekseni se�ene�i

    [HideInInspector] public bool rotate = true; // D�nme �zelli�i aktif 
    [HideInInspector] public bool clockwise = false;            // Saat y�n�nde mi d�ns�n
    [HideInInspector] public float rotationSpeed = 20f;        // D�n�� h�z�
    [HideInInspector] public RotationAxis rotationAxis = RotationAxis.Y; // D�nme ekseni se�imi

    [HideInInspector] public bool move = true;                 // Hareket �zelli�i aktif mi
    [HideInInspector] public bool pauseBetweenMoves = false;   // Hareketlerde duraklama olacak m�
    [HideInInspector] public float moveDistance = 4f;          // Hareket mesafesi
    [HideInInspector] public float moveSpeed = 2f;             // Hareket h�z�
    [HideInInspector] public float pauseDuration = 1f;         // Duraklama s�resi

    private Vector3 startPosition;           // Ba�lang�� pozisyonu
    private bool movingRight = true;         // Hareket y�n�

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


