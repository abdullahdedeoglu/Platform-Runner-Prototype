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

    private void Start()
    {
        InitializePaintingMode();
    }

    public void InitializePaintingMode()
    {
        Debug.Log("Initializing Painting Mode...");
        wallPainter.InitializePainter(); // WallPainter'ý baþlat
        paintingUI.InitializeUI();      // UI'yi baþlat
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
