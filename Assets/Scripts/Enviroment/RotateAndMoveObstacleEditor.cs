#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
// Custom Editor class for the RotateAndMoveObstacle script
[CustomEditor(typeof(RotateAndMoveObstacle))]
public class RotateAndMoveObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the RotateAndMoveObstacle script
        RotateAndMoveObstacle script = (RotateAndMoveObstacle)target;

        // Rotation Settings
        script.rotate = EditorGUILayout.Toggle("Rotate", script.rotate); // Toggle for enabling/disabling rotation
        if (script.rotate) // If rotation is enabled, display additional options
        {
            EditorGUI.indentLevel++; // Indent the fields for better organization in the Inspector
            script.clockwise = EditorGUILayout.Toggle("Clockwise", script.clockwise); // Toggle for clockwise or counterclockwise rotation
            script.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", script.rotationSpeed); // Field to set rotation speed
            script.rotationAxis = (RotateAndMoveObstacle.RotationAxis)EditorGUILayout.EnumPopup("Rotation Axis", script.rotationAxis); // Dropdown to select rotation axis
            EditorGUI.indentLevel--; // Remove the indentation
        }

        // Movement Settings
        script.move = EditorGUILayout.Toggle("Move", script.move); // Toggle for enabling/disabling movement
        if (script.move) // If movement is enabled, display additional options
        {
            EditorGUI.indentLevel++; // Indent the fields for better organization in the Inspector
            script.moveDistance = EditorGUILayout.FloatField("Move Distance", script.moveDistance); // Field to set the movement distance
            script.moveSpeed = EditorGUILayout.FloatField("Move Speed", script.moveSpeed); // Field to set the movement speed
            script.pauseBetweenMoves = EditorGUILayout.Toggle("Pause Between Moves", script.pauseBetweenMoves); // Toggle for enabling/disabling pauses between movements

            if (script.pauseBetweenMoves) // If pauses are enabled, display the pause duration option
            {
                EditorGUI.indentLevel++; // Indent further for nested options
                script.pauseDuration = EditorGUILayout.FloatField("Pause Duration", script.pauseDuration); // Field to set the pause duration
                EditorGUI.indentLevel--; // Remove the extra indentation
            }
            EditorGUI.indentLevel--; // Remove the indentation
        }

        // Draw all other fields of the script that are not explicitly customized
        DrawDefaultInspector();
    }
}
#endif
