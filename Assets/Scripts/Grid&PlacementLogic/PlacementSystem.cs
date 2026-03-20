using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance;

    [SerializeField] private Grid grid;
    [SerializeField] private PathDataSO pathData;
    [SerializeField] private ObjectDatabaseSO database;
    [SerializeField] private GameObject gridVisual;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;

    private Vector3Int lastDetectedPos = Vector3Int.zero;
    private GridData powerData, towerData;

    IBuildingState buildingState;

    public bool isActive => buildingState != null;

    private Dictionary<int, (int id, Vector3Int gridPos)> placedTowerRegistry = new();
    private int activePlacementID = -1;

    void Awake() => Instance = this;

    private void Start()
    {
        StopPlacement();
        gridVisual.SetActive(false);
        powerData = new();
        towerData = new();

        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
        {
            SaveData data = SaveManager.Instance.Load();
            RestoreTowers(data.towers);
        }
    }

    public List<PlacedTowerData> GetPlacedTowerData()
    {
        var list = new List<PlacedTowerData>();
        foreach (var kvp in placedTowerRegistry)
        {
            list.Add(new PlacedTowerData
            {
                towerID = kvp.Value.id,
                gridPos = new Vector3IntSerializable(kvp.Value.gridPos)
            });
        }
        return list;
    }

    private void RestoreTowers(List<PlacedTowerData> towers)
    {
        foreach (var entry in towers)
        {
            int objIndex = database.objectData.FindIndex(d => d.ID == entry.towerID);
            if (objIndex < 0)
            {
                Debug.LogWarning($"[PlacementSystem] No Tower with ID {entry.towerID} in database. Skipping");
                continue;
            }

            ObjectData objData = database.objectData[objIndex];
            Vector3Int gridPos = entry.gridPos.ToVector3Int();
            Vector3 worldPos = grid.CellToWorld(gridPos);

            int index = objectPlacer.PlaceObject(objData.Prefab, worldPos);

            GameObject placedObj = objectPlacer.GetPlacedObject(index);
            Tower tower = placedObj.GetComponent<Tower>();
            if (tower != null)
                tower.Init(gridPos, index);

            towerData.AddObjectAt(gridPos, objData.Size, objData.ID, index);
            placedTowerRegistry[index] = (objData.ID, gridPos);
        }
    }

    public void StartPlacement(int ID)
    {
        activePlacementID = ID;
        TowerSelector.Instance.Deselect();
        StopPlacement();
        gridVisual.SetActive(true);
        buildingState = new PlacementState(ID, grid, preview, database, powerData, towerData, pathData, objectPlacer);
        InputManager.Instance.OnClick += PlaceObject;
        InputManager.Instance.OnCancel += StopPlacement;
    }

    private void PlaceObject()
    {
        if (InputManager.Instance.IsPointerOverUI())
            return;
        Vector3 mousePos = InputManager.Instance.GetMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        int countBefore = objectPlacer.PlacedCount;
        buildingState.OnAction(gridPos);
        int countAfter = objectPlacer.PlacedCount;

        if (countAfter > countBefore)
        {
            int newIndex = countAfter - 1;
            placedTowerRegistry[newIndex] = (activePlacementID, gridPos);
        }
    }

    public void RemoveObject(Tower tower)
    {
        Vector3Int gridPos = tower.GridPosition;
        int index = towerData.GetRepIndex(gridPos);
        if (index == -1) return;

        towerData.RemoveObjectAt(gridPos);
        objectPlacer.RemoveObjectAt(index);
        placedTowerRegistry.Remove(index);
    }

    public void StopPlacement()
    {
        if (buildingState == null) return;
        gridVisual.SetActive(false);
        buildingState.EndState();
        InputManager.Instance.OnClick -= PlaceObject;
        InputManager.Instance.OnCancel -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;
        Vector3 mousePos = InputManager.Instance.GetMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPos = gridPos;
        }
    }
}
