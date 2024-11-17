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

    private void Start()
    {
        InitializePaintingMode();
    }

    public void InitializePaintingMode()
    {
        Debug.Log("Initializing Painting Mode...");
        wallPainter.InitializePainter(); // WallPainter'� ba�lat
        paintingUI.InitializeUI();      // UI'yi ba�lat
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
