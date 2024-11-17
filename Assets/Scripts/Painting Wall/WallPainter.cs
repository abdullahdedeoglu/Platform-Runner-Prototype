using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private Transform brush;       // F�r�a objesi
    [SerializeField] private Transform wallPlane;   // Boyanacak duvar
    [SerializeField] private Renderer wallRenderer; // Duvar�n Renderer'�

    private Color currentColor = Color.red;         // Varsay�lan renk
    private float brushSize = 1f;                   // Varsay�lan f�r�a boyutu

    private Texture2D wallTexture;                 // Duvar�n boyanabilir Texture'u
    private Vector2 textureSize;                   // Texture boyutlar�

    public void InitializePainter()
    {
        Debug.LogWarning("WallPainter Initialization Started");

        // Varsay�lan ayarlar
        currentColor = Color.red;
        brushSize = 20f;

        Debug.LogWarning("Default settings applied: Color = Red, Brush Size = 1");

        // Texture olu�tur
        InitializeWallTexture();
    }

    private void InitializeWallTexture()
    {
        Debug.LogWarning("Initializing Wall Texture...");

        // Mevcut duvar malzemesinin ana Texture'unu al
        Texture mainTexture = wallRenderer.material.mainTexture;

        if (mainTexture is Texture2D)
        {
            wallTexture = Instantiate((Texture2D)mainTexture); // Orijinal texture'un bir kopyas�n� olu�tur
            wallRenderer.material.mainTexture = wallTexture;   // Yeni texture'u duvara uygula
            textureSize = new Vector2(wallTexture.width, wallTexture.height);

            Debug.LogWarning($"Wall texture initialized successfully. Size: {textureSize.x}x{textureSize.y}");
        }
        else
        {
            Debug.LogError("Wall material must use a Texture2D! Texture initialization failed.");
        }
    }

    public void SetBrushColor(Color color)
    {
        currentColor = color;
        Debug.LogWarning($"Brush color updated to: {color}");
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        brush.localScale = new Vector3(size, size, size);
        Debug.LogWarning($"Brush size updated to: {size}");
    }

    private void Update()
    {
        HandleBrushMovement();
    }

    private void HandleBrushMovement()
    {
        if (Input.GetMouseButton(0)) // Sol t�klama kontrol�
        {
            Debug.LogWarning("Mouse button held down. Checking for raycast hit...");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.LogWarning($"Raycast hit detected at: {hit.point}");

                if (hit.transform == wallPlane)
                {
                    Vector3 hitPoint = hit.point;
                    Debug.LogWarning($"Hit wall plane at: {hitPoint}");

                    brush.position = hitPoint + Vector3.up * 0.1f; // F�r�ay� �arpma noktas�na ta��
                    Debug.LogWarning($"Brush moved to: {brush.position}");

                    Paint(hitPoint); // Boyama i�lemini yap
                }
                else
                {
                    //Debug.LogError($"Raycast hit a different object: {hit.transform.name}");
                }
            }
            else
            {
                //Debug.LogError("Raycast did not hit anything.");
            }
        }
    }

    private void Paint(Vector3 position)
    {
        Debug.LogWarning($"Paint method called at position: {position}");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == wallPlane)
            {
                Vector2 pixelUV = hit.textureCoord;
                Debug.LogWarning($"Texture coordinates: {pixelUV}");

                pixelUV.x *= wallTexture.width;
                pixelUV.y *= wallTexture.height;

                Debug.LogWarning($"Converted texture coordinates: {pixelUV}");

                // Texture �zerinde boyama yap
                for (int x = -Mathf.CeilToInt(brushSize); x < Mathf.CeilToInt(brushSize); x++)
                {
                    for (int y = -Mathf.CeilToInt(brushSize); y < Mathf.CeilToInt(brushSize); y++)
                    {
                        int px = (int)pixelUV.x + x;
                        int py = (int)pixelUV.y + y;

                        // Texture s�n�rlar�n�n d���na ta�mamas� i�in kontrol
                        if (px >= 0 && px < wallTexture.width && py >= 0 && py < wallTexture.height)
                        {
                            wallTexture.SetPixel(px, py, currentColor);
                        }
                        else
                        {
                            Debug.LogError($"Pixel ({px}, {py}) is out of bounds. Skipping paint.");
                        }
                    }
                }

                // Texture'u uygula
                wallTexture.Apply();
                Debug.LogWarning("Texture updated and applied.");
            }
            else
            {
                Debug.LogError("Paint method: Raycast hit a different object.");
            }
        }
        else
        {
            Debug.LogError("Paint method: Raycast did not hit anything.");
        }
    }
}
