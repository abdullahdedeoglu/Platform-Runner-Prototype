using UnityEngine;

public class PaintingWallMiniGameManager : MonoBehaviour
{
    public static PaintingWallMiniGameManager Instance { get; private set; }

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



}
