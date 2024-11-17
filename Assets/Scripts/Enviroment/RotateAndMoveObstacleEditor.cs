#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(RotateAndMoveObstacle))]
public class RotateAndMoveObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RotateAndMoveObstacle script = (RotateAndMoveObstacle)target;

        // Rotation Settings
        script.rotate = EditorGUILayout.Toggle("Rotate", script.rotate);
        if (script.rotate)
        {
            EditorGUI.indentLevel++;
            script.clockwise = EditorGUILayout.Toggle("Clockwise", script.clockwise);
            script.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", script.rotationSpeed);
            script.rotationAxis = (RotateAndMoveObstacle.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis", script.rotationAxis);
            EditorGUI.indentLevel--;
        }

        // Movement Settings
        script.move = EditorGUILayout.Toggle("Move", script.move);
        if (script.move)
        {
            EditorGUI.indentLevel++;
            script.moveDistance = EditorGUILayout.FloatField("Move Distance", script.moveDistance);
            script.moveSpeed = EditorGUILayout.FloatField("Move Speed", script.moveSpeed);
            script.pauseBetweenMoves = EditorGUILayout.Toggle("Pause Between Moves", script.pauseBetweenMoves);

            if (script.pauseBetweenMoves)
            {
                EditorGUI.indentLevel++;
                script.pauseDuration = EditorGUILayout.FloatField("Pause Duration", script.pauseDuration);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        // Script'in diðer tüm alanlarýný otomatik çizdir
        DrawDefaultInspector();
    }
}
#endif
