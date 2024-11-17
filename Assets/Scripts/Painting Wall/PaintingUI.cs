using UnityEngine;
using UnityEngine.UI;

public class PaintingUI : MonoBehaviour
{
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Slider sizeSlider;

    public void InitializeUI()
    {
        Debug.Log("Painting UI Initialized");

        // Renk butonlarý
        redButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.red));
        blueButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.blue));
        yellowButton.onClick.AddListener(() => PaintingWallMiniGameManager.Instance.UpdateBrushColor(Color.yellow));

        // Boyut slider
        sizeSlider.onValueChanged.AddListener(size => PaintingWallMiniGameManager.Instance.UpdateBrushSize(size));
    }
}
