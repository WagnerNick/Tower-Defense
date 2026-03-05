using UnityEngine;

public class PathEditor : MonoBehaviour
{
    public Grid grid;
    public PathDataSO pathData;

    public bool paintMode = false;

    private void OnDrawGizmos()
    {
        if (pathData == null || grid == null) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < pathData.path.Count; i++)
        {
            Vector3 pos = grid.GetCellCenterWorld(pathData.path[i]);

            Gizmos.DrawCube(pos, Vector3.one * 0.8f);

            if (i < pathData.path.Count - 1)
            {
                Vector3 next = grid.GetCellCenterWorld(pathData.path[i + 1]);

                Gizmos.DrawLine(pos, next);
            }
        }
    }
}
