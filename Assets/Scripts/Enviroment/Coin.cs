using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public GameObject coinAnimationPrefab; // Animasyon için kullanýlacak coin UI prefab
    public Transform uiTarget; // UI hedef noktasý (örneðin coin sayaç objesi)
    public Canvas canvas; // Ana Canvas referansý
    public int coinValue = 1; // Coin deðeri
    public float animationSpeed = 1f; // Animasyon hýzý (Inspector'dan ayarlanabilir)
    public float animationDelay = 0.1f; // Animasyonlar arasý gecikme

    private RectTransform canvasRectTransform; // Canvas'ýn RectTransform'u
    private bool isCollected = false;

    private MeshRenderer _meshRenderer;

    private List<GameObject> animatedCoins = new List<GameObject>(); // Spawnlanan objeleri tutmak için liste

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

        // Animasyonlarý baþlat
        yield return StartCoroutine(SpawnAndAnimateCoins());

        GameManager.Instance.AddCoin(coinValue);
        // Animasyonlar tamamlandýktan sonra coin objesini yok et
        Destroy(gameObject);
    }

    private IEnumerator SpawnAndAnimateCoins()
    {
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Ekran pozisyonunu Canvas üzerindeki pozisyona dönüþtür
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, cameraPosition, canvas.worldCamera, out Vector2 canvasPosition))
        {
            yield break; // Eðer dönüþtürülemezse iþlem durur
        }

        for (int i = 0; i < 5; i++) // 5 obje yarat
        {
            GameObject animatedCoin = Instantiate(coinAnimationPrefab, canvas.transform);
            RectTransform animatedCoinRect = animatedCoin.GetComponent<RectTransform>();

            if (animatedCoinRect != null)
            {
                animatedCoinRect.anchoredPosition = canvasPosition;

                // Animasyonu baþlat
                animatedCoinRect
                    .DOMove(uiTarget.GetComponent<RectTransform>().position, animationSpeed)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        Destroy(animatedCoin); // Animasyon tamamlandýktan sonra yok et
                    });
            }

            // Her animasyon arasýna gecikme ekle
            yield return new WaitForSeconds(animationDelay);
        }
    }
}
