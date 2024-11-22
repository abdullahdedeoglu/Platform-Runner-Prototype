using UnityEngine;

public class PaintingWallMiniGameManager : MonoBehaviour
{
    public static PaintingWallMiniGameManager Instance { get; private set; } // Singleton instance

    [SerializeField] private WallPainter wallPainter; // Script responsible for wall painting mechanics
    [SerializeField] private PaintingUI paintingUI;   // Script responsible for managing painting-related UI

    private void Awake()
    {
        // Ensure only one instance of the manager exists (Singleton pattern)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Destroy duplicate instances
    }

    // Initializes the painting mode by setting up the painter and UI
    public void InitializePaintingMode()
    {
        Debug.Log("Initializing Painting Mode...");
        wallPainter.InitializePainter(); // Initialize the wall painter functionality
        paintingUI.InitializeUI();      // Initialize the painting UI
    }

    // Checks if the player has achieved 100% painting, triggering win conditions
    public void CheckWinCondition(float paintedPercentage)
    {
        if (paintedPercentage >= 100f) // If the wall is fully painted
        {
            wallPainter.enabled = false;         // Disable the wall painter to stop further painting
            paintingUI.enabled = false;         // Disable the painting UI
            CanvasManager.Instance.ShowWinCanvas(); // Display the win canvas
            SoundManager.Instance.PlayGameWinSound(); // Play a victory sound effect
        }
    }

    // Updates the brush color for the wall painter
    public void UpdateBrushColor(Color color)
    {
        wallPainter.SetBrushColor(color); // Set the new brush color
    }

    // Updates the brush size for the wall painter
    public void UpdateBrushSize(float size)
    {
        wallPainter.SetBrushSize(size); // Set the new brush size
    }
}
