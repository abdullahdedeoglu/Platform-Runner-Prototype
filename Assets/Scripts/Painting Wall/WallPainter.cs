using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private Transform brush;       // Fýrça objesi
    [SerializeField] private Transform wallPlane;   // Boyanacak duvar
    [SerializeField] private Renderer wallRenderer; // Duvarýn Renderer'ý

    private Color currentColor = Color.red;         // Varsayýlan renk
    private float brushSize = 1f;                   // Varsayýlan fýrça boyutu

    private Texture2D wallTexture;                 // Duvarýn boyanabilir Texture'u
    private Vector2 textureSize;                   // Texture boyutlarý

    private int totalPixels;         // Duvarýn toplam piksel sayýsý
    private int paintedPixels = 0;  // Boyanmýþ piksel sayýsý

    public delegate void PaintProgressUpdate(float percentage);
    public static event PaintProgressUpdate OnPaintProgressUpdated;

    public void InitializePainter()
    {

        // Varsayýlan ayarlar
        currentColor = Color.red;
        brushSize = 25f;

        // Texture oluþtur
        InitializeWallTexture();

        // Toplam piksel sayýsýný hesapla
        totalPixels = wallTexture.width * wallTexture.height;
        paintedPixels = 0;
    }

    private void InitializeWallTexture()
    {

        // Mevcut duvar malzemesinin ana Texture'unu al
        Texture mainTexture = wallRenderer.material.mainTexture;

        if (mainTexture is Texture2D)
        {
            wallTexture = Instantiate((Texture2D)mainTexture); // Orijinal texture'un bir kopyasýný oluþtur
            wallRenderer.material.mainTexture = wallTexture;   // Yeni texture'u duvara uygula
            textureSize = new Vector2(wallTexture.width, wallTexture.height);

        }
    }

    public void SetBrushColor(Color color)
    {
        currentColor = color;
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        brush.localScale = new Vector3(size, size, size);
    }

    private void Update()
    {
        HandleBrushMovement();
    }

    private void HandleBrushMovement()
    {
        if (Input.GetMouseButton(0)) // Sol týklama kontrolü
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (hit.transform == wallPlane)
                {
                    Vector3 hitPoint = hit.point;
                    Debug.LogWarning($"Hit wall plane at: {hitPoint}");

                    brush.position = hitPoint + Vector3.up * 0.1f; // Fýrçayý çarpma noktasýna taþý
                    Debug.LogWarning($"Brush moved to: {brush.position}");

                    Paint(hitPoint); // Boyama iþlemini yap
                }
            }
        }
    }

    private void Paint(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == wallPlane)
            {
                Vector2 pixelUV = hit.textureCoord;

                pixelUV.x *= wallTexture.width;
                pixelUV.y *= wallTexture.height;

                for (int x = -Mathf.CeilToInt(brushSize); x < Mathf.CeilToInt(brushSize); x++)
                {
                    for (int y = -Mathf.CeilToInt(brushSize); y < Mathf.CeilToInt(brushSize); y++)
                    {
                        int px = (int)pixelUV.x + x;
                        int py = (int)pixelUV.y + y;

                        if (px >= 0 && px < wallTexture.width && py >= 0 && py < wallTexture.height)
                        {
                            Color pixelColor = wallTexture.GetPixel(px, py);
                            if (pixelColor != currentColor) // Eðer piksel daha önce boyanmamýþsa
                            {
                                wallTexture.SetPixel(px, py, currentColor);
                                paintedPixels++;
                            }
                        }
                    }
                }

                // Texture'u uygula
                wallTexture.Apply();

                // Boyanma yüzdesini hesapla ve event gönder
                float paintedPercentage = (paintedPixels / (float)totalPixels) * 100f;
                OnPaintProgressUpdated?.Invoke(paintedPercentage);
            }
        }
    }
}

