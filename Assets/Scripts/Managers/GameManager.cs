using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance of GameManager

    private int coinCount = 0; // Tracks the total number of coins collected
    public int deathCount = 0; // Tracks the total number of player deaths
    private GameObject floatingJoystick; // Reference to the floating joystick UI element
    public GameMode currentGameMode; // Tracks the current game mode (e.g., Race or PaintingWall)

    private void Awake()
    {
        // Ensure only one instance of GameManager exists (Singleton pattern)
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
        // Find the floating joystick in the scene
        floatingJoystick = GameObject.Find("Floating Joystick");

        // Set the initial game mode to "Race"
        currentGameMode = GameMode.Race;

        // Configure the screen orientation
        SetOrientation();
    }

    // Locks the screen orientation to portrait and allows upside-down rotation
    private void SetOrientation()
    {
        Screen.orientation = ScreenOrientation.Portrait; // Set orientation to Portrait

        // Disable landscape orientations
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

        // Enable portrait orientations
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    // Adds the specified coin value to the total count and updates the UI
    public void AddCoin(int value)
    {
        coinCount += value; // Increment coin count
        CanvasManager.Instance.UpdateCoinUI(coinCount); // Update coin counter in the UI
    }

    // Increments the death count and updates the UI
    public void UpgradeDeathAmount()
    {
        deathCount++; // Increment death count
        CanvasManager.Instance.ShowDeathCount(deathCount); // Update death counter in the UI
    }

    // Called when the player finishes the race phase
    public void OnPlayerFinish()
    {
        SwitchGameMode(GameMode.PaintingWall); // Switch to the "Painting Wall" game mode
    }

    // Switches the game mode and performs related transitions
    private void SwitchGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode; // Update the current game mode

        // Disable the floating joystick for the Painting Wall mode
        floatingJoystick.gameObject.SetActive(false);

        // Smoothly transition the camera to the Painting Wall viewpoint
        StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(
            CameraMovement.Instance.paintingWallCameraPoint.transform.position,
            CameraMovement.Instance.paintingWallCameraPoint.transform.rotation,
            true));

        // Switch the canvas to display the Painting Wall UI
        CanvasManager.Instance.SwitchCanvasses();

        // Enable and initialize the Painting Wall mini-game
        PaintingWallMiniGameManager.Instance.enabled = true;
        PaintingWallMiniGameManager.Instance.InitializePaintingMode();
    }

    // Exits the game (functional only in a built application)
    public void ExitGame()
    {
        Application.Quit();
    }

    // Restarts the game by reloading the first scene
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Reloads the scene at index 0
    }
}

// Enum representing different game modes
public enum GameMode
{
    Race, // Represents the racing phase of the game
    PaintingWall // Represents the wall-painting mini-game phase
}
