using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabaseSO database;
    GridData powerData;
    GridData towerData;
    PathDataSO pathData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabaseSO database,
                          GridData powerData,
                          GridData towerData,
                          PathDataSO pathData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.powerData = powerData;
        this.towerData = towerData;
        this.pathData = pathData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
            previewSystem.ShowPreview(database.objectData[selectedObjectIndex].Prefab, database.objectData[selectedObjectIndex].Size);
        else
            throw new System.Exception($"Object with ID {ID} not found in database");
    }

    public void EndState()
    {
        previewSystem.StopPreview();
    }

    public void OnAction(Vector3Int gridPos)
    {
        bool canPlace = CheckPlacement(gridPos, selectedObjectIndex);
        if (canPlace == false)
            return;

        PlayerMoney.Instance.ChangeMoney(database.objectData[selectedObjectIndex].Cost, false);

        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPos));

        GameObject placedObj = objectPlacer.GetPlacedObject(index);
        Tower tower = placedObj.GetComponent<Tower>();
        if (tower != null)
            tower.Init(gridPos, index);

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? powerData : towerData;
        selectedData.AddObjectAt(gridPos, database.objectData[selectedObjectIndex].Size, database.objectData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePos(grid.CellToWorld(gridPos), false);
    }

    private bool CheckPlacement(Vector3Int gridPos, int selectedObjectIndex)
    {
        Vector2Int size = database.objectData[selectedObjectIndex].Size;
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? powerData : towerData;
        if (PlayerMoney.Instance.money < database.objectData[selectedObjectIndex].Cost)
            return false;
        if (!selectedData.CanPlaceObjectAt(gridPos, size))
            return false;

        if (pathData != null && pathData.cell != null)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int pos = gridPos + new Vector3Int(x, 0, y);
                    if (pathData.cell.Contains(pos))
                        return false;
                }
            }
        }

        return true;
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool canPlace = CheckPlacement(gridPos, selectedObjectIndex);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), canPlace);
    }
}
