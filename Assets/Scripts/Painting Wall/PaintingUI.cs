using UnityEngine;
using UnityEngine.UI;

public class PaintingUI : MonoBehaviour
{
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Slider sizeSlider;



    // Slider aral���n� kontrol edin
    void Start()
    {
        sizeSlider.minValue = 5f; // Minimum f�r�a boyutu
        sizeSlider.maxValue = 150f;  // Maksimum f�r�a boyutu
        sizeSlider.value = 1f;      // Varsay�lan f�r�a boyutu
    }


    public void InitializeUI()
    {
        Debug.Log("Painting UI Initialized");

        redButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.red));
        blueButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.blue));
        yellowButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.yellow));

        sizeSlider.onValueChanged.AddListener(size => PaintingWallMiniGameManager.Instance.UpdateBrushSize(size));

        // Ba�lang�� de�eri olarak %0 g�ster
        CanvasManager.Instance.UpdateProgressText(0f);

        // Boyama ilerlemesi i�in event dinleyicisi ekle
        WallPainter.OnPaintProgressUpdated += CanvasManager.Instance.UpdateProgressText;
    }


    private void OnDestroy()
    {
        WallPainter.OnPaintProgressUpdated -= CanvasManager.Instance.UpdateProgressText;
    }
}
