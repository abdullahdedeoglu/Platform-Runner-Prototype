using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private Transform brush;       // Brush object
    [SerializeField] private Transform wallPlane;   // The wall to paint
    [SerializeField] private Renderer wallRenderer; // Renderer for the wall

    private Color currentColor = Color.red;         // Default brush color
    private float brushSize = 1f;                   // Default brush size

    private Texture2D wallTexture;                 // Texture of the wall for painting
    private Vector2 textureSize;                   // Dimensions of the texture

    private int totalPixels;         // Total number of pixels in the texture
    private int paintedPixels = 0;   // Number of pixels painted

    public delegate void PaintProgressUpdate(float percentage); // Delegate for progress updates
    public static event PaintProgressUpdate OnPaintProgressUpdated;

    public void InitializePainter()
    {
        // Set default brush settings
        currentColor = Color.red;
        brushSize = 75f;

        // Create the texture for painting
        InitializeWallTexture();

        // Calculate the total number of paintable pixels
        totalPixels = (int)(wallTexture.width * wallTexture.height * 0.9999f); // Account for ~1% margin
        paintedPixels = 0;
    }

    private void InitializeWallTexture()
    {
        // Get the main texture from the wall's material
        Texture mainTexture = wallRenderer.material.mainTexture;

        if (mainTexture is Texture2D)
        {
            // Create a copy of the original texture
            wallTexture = Instantiate((Texture2D)mainTexture);

            // Apply the new texture to the wall
            wallRenderer.material.mainTexture = wallTexture;

            textureSize = new Vector2(wallTexture.width, wallTexture.height);
        }
    }

    public void SetBrushColor(Color color)
    {
        currentColor = color; // Update the brush color
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        brush.localScale = new Vector3(size, size, size); // Scale the brush object
    }

    private void Update()
    {
        HandleBrushMovement(); // Update brush position and handle painting
    }

    private void HandleBrushMovement()
    {
        // Check for left mouse button input
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits the wall
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == wallPlane)
                {
                    Vector3 hitPoint = hit.point;

                    // Move the brush to the hit position
                    brush.position = hitPoint + Vector3.up * 0.1f;

                    // Perform the painting action
                    Paint(hitPoint);
                }
            }
        }
    }

    private void Paint(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ensure the ray hits the wall again
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == wallPlane)
            {
                Vector2 pixelUV = hit.textureCoord;

                pixelUV.x *= wallTexture.width;
                pixelUV.y *= wallTexture.height;

                // Paint within the brush's area
                for (int x = -Mathf.CeilToInt(brushSize); x < Mathf.CeilToInt(brushSize); x++)
                {
                    for (int y = -Mathf.CeilToInt(brushSize); y < Mathf.CeilToInt(brushSize); y++)
                    {
                        int px = (int)pixelUV.x + x;
                        int py = (int)pixelUV.y + y;

                        // Ensure the pixel is within bounds
                        if (px >= 0 && px < wallTexture.width && py >= 0 && py < wallTexture.height)
                        {
                            Color pixelColor = wallTexture.GetPixel(px, py);

                            // Count as painted if the pixel was white
                            if (pixelColor == Color.white)
                            {
                                paintedPixels++;
                            }

                            // Set the pixel color to the current brush color
                            wallTexture.SetPixel(px, py, currentColor);
                        }
                    }
                }

                // Apply changes to the texture
                wallTexture.Apply();

                // Calculate and update the painting progress
                float paintedPercentage = (paintedPixels / (float)totalPixels) * 100f;
                OnPaintProgressUpdated?.Invoke(paintedPercentage);

                // Check if the painting is complete
                PaintingWallMiniGameManager.Instance.CheckWinCondition(paintedPercentage);
            }
        }
    }
}
