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

    void Awake() => Instance = this;

    private void Start()
    {
        StopPlacement();
        gridVisual.SetActive(false);
        powerData = new();
        towerData = new();
    }

    public void StartPlacement(int ID)
    {
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

        buildingState.OnAction(gridPos);
    }

    public void RemoveObject(Tower tower)
    {
        Vector3Int gridPos = tower.GridPosition;
        GridData selectedData = towerData;

        int index = selectedData.GetRepIndex(gridPos);
        if (index == -1)
            return;
        selectedData.RemoveObjectAt(gridPos);
        objectPlacer.RemoveObjectAt(index);
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisual.SetActive(false);
        buildingState.EndState();
        InputManager.Instance.OnClick -= PlaceObject;
        InputManager.Instance.OnCancel -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePos = InputManager.Instance.GetMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPos = gridPos;
        }
    }
}
