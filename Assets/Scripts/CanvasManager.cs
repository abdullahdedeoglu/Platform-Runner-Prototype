using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI coinText; // Coin sayaç text'i
    public TextMeshProUGUI deathCountText;

    public GameObject defaultCanvas;
    public GameObject paintWallCanvas;

    private bool switchCanvas = true;

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

    private void Start()
    {
        SwitchCanvasses();
    }

    public void ShowRanking(int ranking)
    {
        rankText.text = ranking + ".";
    }

    public void UpdateCoinUI(int coinCount)
    {
        coinText.text = "Coins: " + coinCount;
    }

    public void ShowDeathCount(int deathCount)
    {
        deathCountText.text = "Death Count\n" + deathCount;
    }

    public void SwitchCanvasses()
    {
        switchCanvas = !switchCanvas;
        defaultCanvas.SetActive(!switchCanvas);
        paintWallCanvas.SetActive(switchCanvas);
    }
}
