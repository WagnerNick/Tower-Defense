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
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabaseSO database,
                          GridData powerData,
                          GridData towerData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.powerData = powerData;
        this.towerData = towerData;
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

        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPos));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? powerData : towerData;
        selectedData.AddObjectAt(gridPos, database.objectData[selectedObjectIndex].Size, database.objectData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePos(grid.CellToWorld(gridPos), false);
    }

    private bool CheckPlacement(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? powerData : towerData;
        return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool canPlace = CheckPlacement(gridPos, selectedObjectIndex);
        previewSystem.UpdatePos(grid.CellToWorld(gridPos), canPlace);
    }
}
