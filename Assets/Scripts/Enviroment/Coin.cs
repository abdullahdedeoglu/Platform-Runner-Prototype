using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public GameObject coinAnimationPrefab; // Animasyon i�in kullan�lacak coin UI prefab
    public Transform uiTarget; // UI hedef noktas� (�rne�in coin saya� objesi)
    public Canvas canvas; // Ana Canvas referans�
    public int coinValue = 1; // Coin de�eri
    public float animationSpeed = 1f; // Animasyon h�z� (Inspector'dan ayarlanabilir)
    public float animationDelay = 0.1f; // Animasyonlar aras� gecikme

    private RectTransform canvasRectTransform; // Canvas'�n RectTransform'u
    private bool isCollected = false;

    private MeshRenderer _meshRenderer;

    private List<GameObject> animatedCoins = new List<GameObject>(); // Spawnlanan objeleri tutmak i�in liste

    private void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        _meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(CollectCoin());
        }
    }

    private IEnumerator CollectCoin()
    {
        //Mesh'i kapat
        _meshRenderer.enabled = false;

        // Animasyonlar� ba�lat
        yield return StartCoroutine(SpawnAndAnimateCoins());

        GameManager.Instance.AddCoin(coinValue);
        // Animasyonlar tamamland�ktan sonra coin objesini yok et
        Destroy(gameObject);
    }

    private IEnumerator SpawnAndAnimateCoins()
    {
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Ekran pozisyonunu Canvas �zerindeki pozisyona d�n��t�r
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, cameraPosition, canvas.worldCamera, out Vector2 canvasPosition))
        {
            yield break; // E�er d�n��t�r�lemezse i�lem durur
        }

        for (int i = 0; i < 5; i++) // 5 obje yarat
        {
            GameObject animatedCoin = Instantiate(coinAnimationPrefab, canvas.transform);
            RectTransform animatedCoinRect = animatedCoin.GetComponent<RectTransform>();

            if (animatedCoinRect != null)
            {
                animatedCoinRect.anchoredPosition = canvasPosition;

                // Animasyonu ba�lat
                animatedCoinRect
                    .DOMove(uiTarget.GetComponent<RectTransform>().position, animationSpeed)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        Destroy(animatedCoin); // Animasyon tamamland�ktan sonra yok et
                    });
            }

            // Her animasyon aras�na gecikme ekle
            yield return new WaitForSeconds(animationDelay);
        }
    }
}
