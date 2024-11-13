using UnityEngine;
using System.Collections;

public class HalfDonutMovement : MonoBehaviour
{
    public float moveDistance = 4f;      // Engel hareket mesafesi
    public float speed = 2f;             // Engel hareket hýzý
    public float pauseDuration = 1f;     // Duraklama süresi
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
                // Hareket saða
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

            // Baþlangýç pozisyonunu güncelle ve yönü deðiþtir
            startPos = transform.position;
            movingRight = !movingRight;
        }
    }
}
