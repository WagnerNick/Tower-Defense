using UnityEngine;

public class PathEditor : MonoBehaviour
{
    public Grid grid;
    public PathDataSO pathData;

    public bool paintMode = false;

    public Vector3Int startCell;
    public Vector3Int endCell;

    private void OnDrawGizmos()
    {
        if (pathData == null || grid == null) return;

        for (int i = 0; i < pathData.cell.Count; i++)
        {
            Vector3 pos = grid.GetCellCenterWorld(pathData.cell[i]);

            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(pos, Vector3.one * 0.8f);

            if (i < pathData.cell.Count - 1)
            {
                Vector3 next = grid.GetCellCenterWorld(pathData.cell[i + 1]);
                Gizmos.DrawLine(pos, next);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(grid.GetCellCenterWorld(startCell), 0.4f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(grid.GetCellCenterWorld(endCell), 0.4f);
    }
}
