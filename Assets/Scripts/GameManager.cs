using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int coinCount = 0;
    public int deathCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddCoin(int value)
    {
        coinCount += value;

        // UI'yi güncelle
        CanvasManager.Instance.UpdateCoinUI(coinCount);
    }

    public void UpgradeDeathAmount()
    {
        deathCount++;
        CanvasManager.Instance.ShowDeathCount(deathCount);
    }
}
