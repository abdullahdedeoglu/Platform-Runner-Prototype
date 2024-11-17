using UnityEngine;

public class PaintingWallMiniGameManager : MonoBehaviour
{
    public static PaintingWallMiniGameManager Instance { get; private set; }

    [SerializeField] private WallPainter wallPainter; // Duvar boyama i�lemini yapan script
    [SerializeField] private PaintingUI paintingUI;   // UI kontrol� i�in script

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitializePaintingMode()
    {
        Debug.Log("Initializing Painting Mode...");
        wallPainter.InitializePainter(); // WallPainter'� ba�lat
        paintingUI.InitializeUI();      // UI'yi ba�lat
    }

    public void CheckWinCondition(float paintedPercentage)
    {
        if (paintedPercentage >= 100f) // %100 boyand�ysa
        {
            wallPainter.enabled = false; // Duvar boyamay� durdur
            paintingUI.enabled = false;     // UI'y� devre d��� b�rak
            CanvasManager.Instance.ShowWinCanvas();         // Kazanma ekran�n� g�ster
        }
    }

    public void UpdateBrushColor(Color color)
    {
        wallPainter.SetBrushColor(color);
    }

    public void UpdateBrushSize(float size)
    {
        wallPainter.SetBrushSize(size);
    }
}
