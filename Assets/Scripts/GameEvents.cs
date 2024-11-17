using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<Color> OnColorChange;
    public static Action<float> OnBrushSizeChange;
}
