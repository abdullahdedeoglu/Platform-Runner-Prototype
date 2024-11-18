using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public TextMeshProUGUI rankText;
    public TextMeshProUGUI coinText; // Coin sayaç text'i
    public TextMeshProUGUI deathCountText;

    [SerializeField] private TextMeshProUGUI progressText;

    public GameObject defaultCanvas;
    public GameObject paintWallCanvas;
    public GameObject winCanvas;

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
        winCanvas.SetActive(false);
        SwitchCanvasses();
    }

    public void ShowRanking(int ranking)
    {
        rankText.text = ranking + ".";
    }

    public void UpdateCoinUI(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }

    public void ShowDeathCount(int deathCount)
    {
        deathCountText.text = deathCount.ToString();
    }
    public void UpdateProgressText(float percentage)
    {
        progressText.text = "% " + (int)percentage;

        if (percentage < 50f)
            progressText.color = Color.yellow;
        else
            progressText.color = Color.green;
    }
    public void ShowWinCanvas()
    {
        winCanvas.SetActive(true);
    }
    public void SwitchCanvasses()
    {
        switchCanvas = !switchCanvas;
        defaultCanvas.SetActive(!switchCanvas);
        paintWallCanvas.SetActive(switchCanvas);
    }

}
