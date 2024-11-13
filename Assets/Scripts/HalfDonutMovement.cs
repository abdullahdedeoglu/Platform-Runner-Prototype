using UnityEngine;
using System.Collections;

public class HalfDonutMovement : MonoBehaviour
{
    public float moveDistance = 4f;      // Engel hareket mesafesi
    public float speed = 2f;             // Engel hareket h�z�
    public float pauseDuration = 1f;     // Duraklama s�resi
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveHalfDonut());
    }

    private IEnumerator MoveHalfDonut()
    {
        while (true)
        {
            if (!movingRight)
            {
                // Hareket sa�a
                while (Vector3.Distance(startPos, transform.position) < moveDistance)
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                // Hareket sola
                while (Vector3.Distance(startPos, transform.position) < moveDistance)
                {
                    transform.position -= Vector3.right * speed * Time.deltaTime;
                    yield return null;
                }
            }

            // Duraklama
            yield return new WaitForSeconds(pauseDuration);

            // Ba�lang�� pozisyonunu g�ncelle ve y�n� de�i�tir
            startPos = transform.position;
            movingRight = !movingRight;
        }
    }
}
