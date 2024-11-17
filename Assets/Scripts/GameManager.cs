using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int coinCount = 0;
    public int deathCount = 0;
    private GameObject floatingJoystick;

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
    }

    public void AddCoin(int value)
    {
        coinCount += value;

        // UI'yi güncelle
        CanvasManager.Instance.UpdateCoinUI(coinCount);
    }

    public void UpgradeDeathAmount()
    {
        deathCount++;
        CanvasManager.Instance.ShowDeathCount(deathCount);
    }

    public void OnPlayerFinish()
    {
        Debug.Log("We reached the end line!");
        floatingJoystick.gameObject.SetActive(false);

        StartCoroutine(CameraMovement.Instance.SmoothTransitionTo(
            CameraMovement.Instance.paintingWallCameraPoint.transform.position,
            CameraMovement.Instance.paintingWallCameraPoint.transform.rotation,
            true));

        CanvasManager.Instance.SwitchCanvasses();

        //TODO
    }
}
