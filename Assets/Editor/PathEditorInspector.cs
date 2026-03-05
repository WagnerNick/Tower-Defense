using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathEditor))]
public class PathEditorInspector : Editor
{
    PathEditor editor;

    private void OnEnable()
    {
        editor = (PathEditor)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button(editor.paintMode ? "Stop Painting" : "Start Painting"))
        {
            editor.paintMode = !editor.paintMode;
        }

        if (GUILayout.Button("Clear Path"))
        {
            Undo.RecordObject(editor.pathData, "Clear Path");
            editor.pathData.path.Clear();
            EditorUtility.SetDirty(editor.pathData);
        }
    }

    private void OnSceneGUI()
    {
        if (!editor.paintMode) return;
        if (editor.pathData == null || editor.grid == null) return;

        Event e = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);

            Vector3Int cell = editor.grid.WorldToCell(worldPos);
            Vector3 cellCenter = editor.grid.GetCellCenterWorld(cell);

            Handles.color = Color.green;
            Handles.DrawWireCube(cellCenter, Vector3.one);

            bool left = e.button == 0;
            bool right = e.button == 1;

            if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
            {
                if (left)
                {
                    if (!editor.pathData.path.Contains(cell))
                    {
                        Undo.RecordObject(editor.pathData, "Add Path Node");
                        editor.pathData.path.Add(cell);
                        EditorUtility.SetDirty(editor.pathData);
                    }
                    e.Use();
                }

                if (right)
                {
                    if (editor.pathData.path.Contains(cell))
                    {
                        Undo.RecordObject(editor.pathData, "Remove Path Node");
                        editor.pathData.path.Remove(cell);
                        EditorUtility.SetDirty(editor.pathData);
                    }
                    e.Use();
                }
            }
        }
    }
}
