using UnityEngine;
using UnityEngine.UI;

public class PaintingUI : MonoBehaviour
{
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Slider sizeSlider;



    // Slider aralýðýný kontrol edin
    void Start()
    {
        sizeSlider.minValue = 5f; // Minimum fýrça boyutu
        sizeSlider.maxValue = 150f;  // Maksimum fýrça boyutu
        sizeSlider.value = 1f;      // Varsayýlan fýrça boyutu
    }


    public void InitializeUI()
    {
        Debug.Log("Painting UI Initialized");

        redButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.red));
        blueButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.blue));
        yellowButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.yellow));

        sizeSlider.onValueChanged.AddListener(size => PaintingWallMiniGameManager.Instance.UpdateBrushSize(size));

        // Baþlangýç deðeri olarak %0 göster
        CanvasManager.Instance.UpdateProgressText(0f);

        // Boyama ilerlemesi için event dinleyicisi ekle
        WallPainter.OnPaintProgressUpdated += CanvasManager.Instance.UpdateProgressText;
    }


    private void OnDestroy()
    {
        WallPainter.OnPaintProgressUpdated -= CanvasManager.Instance.UpdateProgressText;
    }
}
