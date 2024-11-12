using UnityEngine;

public class SetOrientation : MonoBehaviour
{
    void Start()
    {
        // Oyun başladığında dikey moda ayarlar
        Screen.orientation = ScreenOrientation.Portrait;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }
}
