using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int coinCount = 0;
    public int deathCount = 0;
    private GameObject floatingJoystick;
    public GameMode currentGameMode;

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
        floatingJoystick = GameObject.Find("Floating Joystick");
        currentGameMode = GameMode.Race;
        SetOrientation();
    }

    private void SetOrientation()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    public void AddCoin(int value)
    {
        coinCount += value;

        CanvasManager.Instance.UpdateCoinUI(coinCount);
    }

    public void UpgradeDeathAmount()
    {
        deathCount++;
        CanvasManager.Instance.ShowDeathCount(deathCount);
    }

    public void OnPlayerFinish()
    {
        
        SwitchGameMode(GameMode.PaintingWall);

    }

    private void SwitchGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;

        floatingJoystick.gameObject.SetActive(false);

        StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(
            CameraMovement.Instance.paintingWallCameraPoint.transform.position,
            CameraMovement.Instance.paintingWallCameraPoint.transform.rotation,
            true));

        CanvasManager.Instance.SwitchCanvasses();

        PaintingWallMiniGameManager.Instance.enabled = true;
        PaintingWallMiniGameManager.Instance.InitializePaintingMode();

    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}

public enum GameMode
{
    Race,
    PaintingWall
}
