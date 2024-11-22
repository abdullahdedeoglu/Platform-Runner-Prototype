using UnityEngine;
using UnityEngine.UI;

public class PaintingUI : MonoBehaviour
{
    [SerializeField] private Button redButton;    // Button for selecting the red brush
    [SerializeField] private Button blueButton;   // Button for selecting the blue brush
    [SerializeField] private Button yellowButton; // Button for selecting the yellow brush
    [SerializeField] private Slider sizeSlider;   // Slider for adjusting brush size

    // Configure the slider range
    void Start()
    {
        sizeSlider.minValue = 5f;    // Minimum brush size
        sizeSlider.maxValue = 150f;  // Maximum brush size
        sizeSlider.value = 1f;       // Default brush size
    }

    public void InitializeUI()
    {
        Debug.Log("Painting UI Initialized");

        // Add listeners for color selection buttons
        redButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.red));
        blueButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.blue));
        yellowButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.yellow));

        // Add a listener for the brush size slider
        sizeSlider.onValueChanged.AddListener(size => PaintingWallMiniGameManager.Instance.UpdateBrushSize(size));

        // Initialize progress text to 0%
        CanvasManager.Instance.UpdateProgressText(0f);

        // Subscribe to the painting progress update event
        WallPainter.OnPaintProgressUpdated += CanvasManager.Instance.UpdateProgressText;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the painting progress update event
        WallPainter.OnPaintProgressUpdated -= CanvasManager.Instance.UpdateProgressText;
    }
}
