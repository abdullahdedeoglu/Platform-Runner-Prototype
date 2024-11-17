using UnityEngine;

public class PaintingWallMiniGameManager : MonoBehaviour
{
    public static PaintingWallMiniGameManager Instance { get; private set; }

    [SerializeField] private WallPainter wallPainter; // Duvar boyama iþlemini yapan script
    [SerializeField] private PaintingUI paintingUI;   // UI kontrolü için script

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
        wallPainter.InitializePainter(); // WallPainter'ý baþlat
        paintingUI.InitializeUI();      // UI'yi baþlat
    }

    public void CheckWinCondition(float paintedPercentage)
    {
        if (paintedPercentage >= 100f) // %100 boyandýysa
        {
            wallPainter.enabled = false; // Duvar boyamayý durdur
            paintingUI.enabled = false;     // UI'yý devre dýþý býrak
            CanvasManager.Instance.ShowWinCanvas();         // Kazanma ekranýný göster
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
