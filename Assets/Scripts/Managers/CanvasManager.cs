using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; } // Singleton instance of the CanvasManager

    // UI elements for displaying various stats
    public TextMeshProUGUI rankText; // Text for displaying the player's rank
    public TextMeshProUGUI coinText; // Text for displaying the coin counter
    public TextMeshProUGUI deathCountText; // Text for displaying the death count

    [SerializeField] private TextMeshProUGUI progressText; // Text for displaying progress as a percentage

    // Canvas objects to manage different game states
    public GameObject defaultCanvas; // The default game canvas
    public GameObject paintWallCanvas; // Canvas used for wall-painting mode
    public GameObject winCanvas; // Canvas displayed when the player wins

    private bool switchCanvas = true; // State to toggle between default and paint wall canvases

    private void Awake()
    {
        // Ensure only one instance of the CanvasManager exists (Singleton pattern)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        winCanvas.SetActive(false); // Ensure the win canvas is inactive at the start of the game
        SwitchCanvasses(); // Initialize canvas visibility
    }

    // Updates the rank display with the player's current rank
    public void ShowRanking(int ranking)
    {
        rankText.text = ranking + "."; // Display rank with a period (e.g., "1.")
    }

    // Updates the coin counter UI with the current coin count
    public void UpdateCoinUI(int coinCount)
    {
        coinText.text = coinCount.ToString(); // Convert coin count to string and display it
    }

    // Updates the death count display with the current number of deaths
    public void ShowDeathCount(int deathCount)
    {
        deathCountText.text = deathCount.ToString(); // Convert death count to string and display it
    }

    // Updates the progress percentage and changes text color based on progress
    public void UpdateProgressText(float percentage)
    {
        progressText.text = "% " + (int)percentage; // Display progress as an integer percentage (e.g., "50%")

        // Change text color based on the percentage
        if (percentage < 50f)
            progressText.color = Color.yellow; // Less than 50%: Yellow
        else
            progressText.color = Color.green; // 50% or more: Green
    }

    // Activates the win canvas when the player wins
    public void ShowWinCanvas()
    {
        winCanvas.SetActive(true);
    }

    // Toggles visibility between the default canvas and the paint wall canvas
    public void SwitchCanvasses()
    {
        switchCanvas = !switchCanvas; // Toggle the canvas state
        defaultCanvas.SetActive(!switchCanvas); // Activate or deactivate the default canvas
        paintWallCanvas.SetActive(switchCanvas); // Activate or deactivate the paint wall canvas
    }
}
