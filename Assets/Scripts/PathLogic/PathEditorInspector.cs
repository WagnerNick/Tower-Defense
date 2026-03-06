using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathEditor))]
public class PathEditorInspector : Editor
{
    PathEditor editor;
    Vector3Int? lastPainted;

    private void OnEnable()
    {
        editor = (PathEditor)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button(editor.paintMode ? "Stop Painting" : "Start Painting"))
            editor.paintMode = !editor.paintMode;

        if (GUILayout.Button("Set Start at First Node"))
        {
            if (editor.pathData.cell.Count > 0)
                editor.startCell = editor.pathData.cell[0];
        }

        if (GUILayout.Button("Set End at Last Node"))
        {
            if (editor.pathData.cell.Count > 0)
                editor.endCell = editor.pathData.cell[^1];
        }

        if (GUILayout.Button("Auto Order Path"))
        {
            HashSet<Vector3Int> nodes = new(editor.pathData.cell);

            var ordered = PathUtils.OrderPath(nodes, editor.startCell);

            Undo.RecordObject(editor.pathData, "Order Path");
            editor.pathData.cell = ordered;
            EditorUtility.SetDirty(editor.pathData);
        }

        if (GUILayout.Button("Clear Path"))
        {
            Undo.RecordObject(editor.pathData, "Clear Path");
            editor.pathData.cell.Clear();
            EditorUtility.SetDirty(editor.pathData);
        }
    }

    private void OnSceneGUI()
    {
        if (!editor.paintMode || editor.grid == null || editor.pathData == null) return;

        Event e = Event.current;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (!plane.Raycast(ray, out float enter))
            return;

        Vector3 world = ray.GetPoint(enter);
        Vector3Int cell = editor.grid.WorldToCell(world);
        Vector3 center = editor.grid.GetCellCenterWorld(cell);

        Handles.color = Color.green;
        Handles.DrawWireCube(center, Vector3.one);

        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
        {
            if (e.button == 0)
            {
                AddCell(cell, e.shift);
                e.Use();
            }

            if (e.button == 1)
            {
                RemoveCell(cell);
                e.Use();
            }
        }
    }

    private void AddCell(Vector3Int cell, bool straight)
    {
        Undo.RecordObject(editor.pathData, "Add Path");

        if (straight && lastPainted.HasValue)
        {
            Vector3Int start = lastPainted.Value;
            Vector3Int diff = cell - start;

            int steps = Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.z));

            for (int i = 1; i <= steps; i++)
            {
                Vector3Int step = new Vector3Int(
                    start.x + diff.x * i / steps,
                    start.y,
                    start.z + diff.z * i / steps
                    );

                if (!editor.pathData.cell.Contains(step))
                    editor.pathData.cell.Add(step);
            }
        }
        else
        {
            if (!editor.pathData.cell.Contains(cell))
                editor.pathData.cell.Add(cell);
        }
        lastPainted = cell;
        EditorUtility.SetDirty(editor.pathData);
    }

    private void RemoveCell(Vector3Int cell)
    {
        Undo.RecordObject(editor.pathData, "Remove Path");
        editor.pathData.cell.Remove(cell);
        EditorUtility.SetDirty(editor.pathData);
    }
}
