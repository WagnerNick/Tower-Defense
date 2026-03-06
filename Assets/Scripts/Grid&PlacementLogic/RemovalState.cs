using System;
using UnityEngine;

public class RemovalState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData powerData;
    GridData towerData;
    ObjectPlacer objectPlacer;

    public RemovalState(Grid grid,
                        PreviewSystem previewSystem,
                        GridData powerData,
                        GridData towerData,
                        ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.powerData = powerData;
        this.towerData = towerData;
        this.objectPlacer = objectPlacer;

        previewSystem.ShowRemovalPreview();
    }

    public void EndState()
    {
        previewSystem.StopPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        GridData selectedData = null;
        if (!towerData.CanPlaceObjectAt(gridPos, Vector2Int.one))
            selectedData = towerData;
        else if (!powerData.CanPlaceObjectAt(gridPos, Vector2Int.one))
            selectedData = powerData;

        if (selectedData == null)
            return;
        else
        {
            gameObjectIndex = selectedData.GetRepIndex(gridPos);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPos);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPos = grid.CellToWorld(gridPos);
        previewSystem.UpdatePos(cellPos, CheckIfValid(gridPos));
    }

    private bool CheckIfValid(Vector3Int gridPos)
    {
        return !(towerData.CanPlaceObjectAt(gridPos, Vector2Int.one) && powerData.CanPlaceObjectAt(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool valid = CheckIfValid(gridPos);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), valid);
    }
}
